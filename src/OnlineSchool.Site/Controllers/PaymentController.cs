using ItalWebConsulting.Infrastructure.Comunication;
using ItalWebConsulting.Infrastructure.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Core.Courses_;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Payment_.Service;
using OnlineSchool.Core.SpotMeeting_.Service;
using OnlineSchool.Core.Students_.Service;
using Stripe;
using System;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Controllers
{
    public class PaymentController : ControllerBase<PaymentController>
    {
        private readonly IConfiguration Configuration;
        public IStudentService StudentService { get; set; }
        public ISubjectService SubjectService { get; set; }
        public IEmailService EmailService { get; set; }
        public ICourseService CourseService { get; set; }
        public ISpotMeetingService SpotMeetingService { get; set; }
        public IPaymentService PaymentService { get; set; }
        public IIdentityService IdentityService { get; set; }
        public PurchasableItemFactory PurchasableItemFactory { get; set; }

        public PaymentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
     
        [HttpPost]
        public async Task<IActionResult> Charge(string stripeToken, int id, PurchasableItemType purchasableItemType)
        {
            var email = GetUser().Identity.Name;

            var service = PurchasableItemFactory.GetPurchasableItemService(purchasableItemType);
            var result = await service.BuyAsync(stripeToken, id, email);

            return Json(result);
        }

        [Authorize]
        public async Task<IActionResult> Index(int id, PurchasableItemType purchasableItemType)
        {
            switch (purchasableItemType)
            {
                case PurchasableItemType.Course:
                    if (!User.IsInRole("Student"))
                        return RedirectToAction("Details", "Course", new { id = id});
                    break;
                case PurchasableItemType.SpotMeeting:
                    break;
                default:
                    break;
            }

            var service = PurchasableItemFactory.GetPurchasableItemService(purchasableItemType);

            var isPurchased = await service.GetExistingPurchasedItemAsync(GetUser().Identity.Name, id);
            if (isPurchased.Output)
            {
                return RedirectToAction("Details", purchasableItemType.ToString(), new { id = id });
            }

            var purchasableItem = await service.GetPurchasableItemByIdAsync(id);

            var stripeKey = Configuration.GetSection("Stripe").GetValue<string>("PublishableKey");
            ViewBag.StripePublishKey = stripeKey;

            return View(purchasableItem);
        }

        [HttpPost]
        public ActionResult Create(PaymentIntentCreateRequest request)
        {
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(request.Items),
                Currency = "eur",
            });
            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

        private int CalculateOrderAmount(Item[] items)
        {
            // Replace this constant with a calculation of the order's amount
            // Calculate the order total on the server to prevent
            // people from directly manipulating the amount on the client
            return 1400;
        }

        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public Item[] Items { get; set; }
        }
    }
}
