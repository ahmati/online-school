using ItalWebConsulting.Infrastructure.BusinessLogic;
using ItalWebConsulting.Infrastructure.Extension;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Webex;
using OnlineSchool.Core.Webex_.Repository;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Webex_.Services
{
    public class WebexService : CoreBase, IWebexService
    {
        public IHttpClientFactory HttpClientFactory { get; set; }
        public IWebexRepository WebexRepository { get; set; }

        public WebexService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public async Task<ResponseBase<MeetingModel>> CreateMeetingAsync(CreateMeetingModel model)
        {
            var output = new ResponseBase<MeetingModel>();
            string errorMessage = "The webex meeting could not be created.";
            try
            {
                var integration = await this.GetIntegrationAsync();
                if (integration is null)
                    throw new NullReferenceException("No integration found in database");
                if (integration.DaysUntilExpiration == 0)
                    throw new InvalidOperationException("Access token from integration has expired. Please, refresh the token");

                var client = HttpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // ACCEPT header
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {integration.AccessToken}");

                var requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var result = await client.PostAsync("https://webexapis.com/v1/meetings", requestBody);
                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.NoContent)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<MeetingModel>(content);

                    output.Output = response;
                }
                else
                    output.AddError(errorMessage);

                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> CheckHasMeetingStartedAsync(string meetingId)
        {
            var output = new ResponseBase<bool>();
            var errorMessage = "An error occurred while checking the meeting status";
            try
            {
                if (string.IsNullOrWhiteSpace(meetingId))
                    throw new ArgumentException("Invalid meetingId");

                var integration = await this.GetIntegrationAsync();
                if(integration is null)
                {
                    output.AddError("No integration information found in database.");
                    return output;
                }

                if(integration.DaysUntilExpiration == 0)
                {
                    output.AddError("Integration access token has expired. Please, refresh the token");
                    return output;
                }

                var client = HttpClientFactory.CreateClient();
                client.BaseAddress = new Uri($"https://webexapis.com/v1/meetings/{meetingId}");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {integration.AccessToken}");
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var data = await client.GetAsync("").Result.Content.ReadAsStringAsync();
                var meeting = JsonConvert.DeserializeObject<MeetingModel>(data);
                var meetingState = meeting.state;
                output.Output = meetingState.Equals(MeetingState.InProgress.GetDescription());

                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred.", ex.Message);
                output.AddError(errorMessage);
                return null;
            }
        }

        public async Task<WebexIntegrationModel> GetIntegrationAsync()
        {
            try
            {
                var data = await WebexRepository.GetIntegrationAsync();
                return Mapper.Map<WebexIntegrationModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred.", ex.Message);
                return null;
            }
        }

        public async Task<ResponseBase<bool>> UpdateIntegrationAsync(WebexIntegrationModel model)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var integration = await WebexRepository.GetIntegrationAsync();
                if (integration is null)
                {
                    output.AddError("Integration could not be found in database.");
                    return output;
                }

                integration.AccessToken = model.AccessToken;
                integration.ExpiresIn = model.ExpiresIn;
                integration.LastUpdated = DateTime.Now;

                output.Output = await WebexRepository.UpdateIntegrationAsync(integration);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred.", ex.Message);
                output.AddError("An error occurred. Webex access token was not be refreshed.");
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteMeetingAsync(string meetingId)
        {
            var output = new ResponseBase<bool>();
            string errorMessage = "The webex meeting could not be deleted.";
            try
            {
                var integration = await this.GetIntegrationAsync();
                if (integration is null)
                    throw new NullReferenceException("No integration found in database");
                if (integration.DaysUntilExpiration == 0)
                    throw new InvalidOperationException("Access token from integration has expired. Please, refresh the token");

                var client = HttpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // ACCEPT header
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {integration.AccessToken}");

                var result = await client.DeleteAsync($"https://webexapis.com/v1/meetings/{meetingId}");
                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.NoContent)
                    output.Output = true;
                else
                    output.AddError(errorMessage);

                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }

        public async Task<WebexGuestIssuerModel> GetGuestIssuerAsync()
        {
            try
            {
                var data = await WebexRepository.GetGuestIssuerAsync();
                return Mapper.Map<WebexGuestIssuerModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred.", ex.Message);
                return null;
            }
        }

        public async Task<ResponseBase<string>> GenerateGuestTokenAsync(string email, string displayName)
        {
            var output = new ResponseBase<string>();
            string errorMessage = "An error occurred. The guest access token could not be generated.";
            try
            {
                var guestIssuer = await this.GetGuestIssuerAsync();

                var dtNow = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var dtExp = (Int32)(DateTime.UtcNow.AddHours(12).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                var payload = new Dictionary<string, object>
                {
                   { "sub", email },
                   { "name", displayName },
                   { "iss", guestIssuer.Id },
                   { "iat", dtNow },
                   { "exp", dtExp }
                };

                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                var secretBytes = Convert.FromBase64String(guestIssuer.Secret);
                var token = encoder.Encode(payload, secretBytes);

                var client = HttpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://webexapis.com");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var response = await client.PostAsync("/v1/jwt/login", null);
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenObject = serializer.Deserialize<dynamic>(responseBody);

                token = tokenObject.token;
                output.Output = token;

                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }
    }
}
