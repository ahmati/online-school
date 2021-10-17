using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.Webex;
using OnlineSchool.Core.Session_.Service;
using OnlineSchool.Core.Students_.Service;
using OnlineSchool.Core.Teachers_.Service;
using OnlineSchool.Core.Webex_.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IW = ItalWebConsulting.Infrastructure.Mvc;

namespace OnlineSchool.Site.Controllers
{
    public class LiveSessionController : IW.ControllerBase
    {
        public IConfiguration Configuration { get; set; }
        public IHttpClientFactory HttpClientFactory { get; set; }
        public ISessionService SessionService { get; set; }
        public IStudentService StudentService { get; set; }
        public ITeacherService TeacherService { get; set; }
        public IWebexService WebexService { get; set; }

        #region ------------------------------------------ Webex Token refresh ------------------------------------------

        public LiveSessionController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            Configuration = configuration;
            HttpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult WebexIntegration()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetWebexIntegration([DataSourceRequest] DataSourceRequest request)
        {
            var integration = await WebexService.GetIntegrationAsync();

            // Returning it as list, even though there is only 1 element, because the grid requires a list
            var list = new List<WebexIntegrationModel>() { integration };

            var result = list.ToDataSourceResult(request);
            return Json(result);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult StartOAuthFlow()
        {
            var integrationUrl = Configuration.GetSection("Webex").GetValue<string>("IntegrationUrl");
            return Redirect(integrationUrl);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RefreshToken(string code)
        {
            var integration = await WebexService.GetIntegrationAsync();

            var client = HttpClientFactory.CreateClient();

            var form = new List<KeyValuePair<string, string>>();
            form.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            form.Add(new KeyValuePair<string, string>("client_id", integration.ClientId));
            form.Add(new KeyValuePair<string, string>("client_secret", integration.ClientSecret));
            form.Add(new KeyValuePair<string, string>("code", code));
            form.Add(new KeyValuePair<string, string>("redirect_uri", integration.RedirectUri));

            var result = await client.PostAsync("https://webexapis.com/v1/access_token", new FormUrlEncodedContent(form));
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<AuthCodeGrantExchangeResponse>(content);

                integration.AccessToken = response.Access_Token;
                integration.ExpiresIn = response.Expires_In;

                var updateResult = await WebexService.UpdateIntegrationAsync(integration);
                if (updateResult.HasErrors)
                    TempData["Error"] = "An error occurred. The Webex token was not refreshed.";
                else
                    TempData["Success"] = "Webex token was refreshed successfully.";
            }
            else
                TempData["Error"] = "An error occurred. The Webex token was not refreshed.";

            return RedirectToAction("WebexIntegration");
        }

        private class AuthCodeGrantExchangeResponse
        {
            public string Access_Token { get; set; }
            public long Expires_In { get; set; }
            public string Refresh_Token { get; set; }
            public long Refresh_Token_Expires_In { get; set; }
        }

        #endregion

        [Authorize(Roles = "Teacher")]
        [HttpGet("[controller]/{sessionId}/Host")]
        public async Task<IActionResult> Host(int sessionId)
        {
            // 1. Check if session with sessionId belongs to you (teacher)
            // 2. Check the session Date/Start/End so that you don't start the session too early or after it has finished.
            // 3. Pass the ciscoAuthToken & meetingSipAddress

            var personalInfo = await TeacherService.GetByEmailAsync(GetUser().Identity.Name);

            // 1.
            var session = await TeacherService.GetSessionByIdAsync(personalInfo.Id, sessionId);
            if (session is null)
                return RedirectToAction("UpcomingSessions", "Teacher");

            // 2.
            if(this.IsSessionReadyToStart(session) == false)
            {
                TempData["Error"] = "Session is not ready to start yet.";
                return RedirectToAction("UpcomingSessions", "Teacher");
            }

            // 3.
            var integration = await WebexService.GetIntegrationAsync();
            ViewBag.Token = integration.AccessToken;
            ViewBag.MeetingSipAddress = session.MeetingSipAddress;

            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpGet("[controller]/{sessionId}/Guest")]
        public async Task<IActionResult> Guest(int sessionId)
        {
            // 1. Check if session with sessionId belongs to you (student)
            // 2. Pass the ciscoAuthToken & meetingSipAddress

            var personalInfo = await StudentService.GetByEmailAsync(GetUser().Identity.Name);

            // 1.
            var session = await StudentService.GetSessionByIdAsync(personalInfo.Id, sessionId);
            if (session is null)
                return RedirectToAction("UpcomingSessions", "Student");

            // 2.
            ViewBag.SessionId = sessionId;
            var guestTokenGen = await WebexService.GenerateGuestTokenAsync(personalInfo.Email, $"{personalInfo.Name} {personalInfo.Surname}");
            if(guestTokenGen.HasErrors)
            {
                TempData["Error"] = guestTokenGen.Errors[0];
                return RedirectToAction("UpcomingSessions", "Student");
            }

            ViewBag.Token = guestTokenGen.Output;
            ViewBag.MeetingSipAddress = session.MeetingSipAddress;
            ViewBag.MeetingPassword = session.MeetingPassword;

            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpGet("[controller]/{sessionId}/[action]")]
        public async Task<IActionResult> ReadyToJoin(int sessionId)
        {
            var response = new ResponseBase<bool>();

            var session = await SessionService.GetByIdAsync(sessionId);
            if (session is null)
            {
                response.AddError("Could not check the status of the session. It may be unavailable or it may not exist.");
                return Json(response);
            }

            var isReadyToJoin = await WebexService.CheckHasMeetingStartedAsync(session.MeetingId);
            if(isReadyToJoin.HasErrors)
            {
                response.AddErrors(isReadyToJoin.Errors);
                return Json(response);
            }

            response.Output = isReadyToJoin.Output;
            return Json(response);
        }

        // An endpoint for serving react-app, as it should be privately served and the endpoint authorized
        [Authorize(Roles = "Student, Teacher")]
        public IActionResult ReactApp()
        {
            return File("react-app/index.html", "text/html");
        }
         
        // ---------------------------------------------- Helpers -------------------------------------------------------
        private bool IsSessionReadyToStart(SessionModel session)
        {
            var now = DateTime.Now.AddMinutes(30);

            var start = session.Date.Date + session.StartTime;
            var end = session.Date.Date + session.EndTime;

            return (now >= start && now <= end);
        }
    }
}
