using ItalWebConsulting.Infrastructure.BusinessLogic;
using ItalWebConsulting.Infrastructure.Comunication;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Core.Courses_;
using OnlineSchool.Core.Students_.Service;
using System;
using Stripe;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Infrastructure;

namespace OnlineSchool.Core.Payment_.Service
{
    public class PaymentService : CoreBase, IPaymentService
    {
        public IConfiguration Configuration { get; set; }
        
        public async Task<ResponseBase<bool>> PayIt(string stripeToken, decimal price, string name, string email)
        {
            if (string.IsNullOrWhiteSpace(stripeToken))
                throw new ArgumentNullException(nameof(stripeToken));
            else if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var result = new ResponseBase<bool>();
            try
            {
                // Set your secret key. Remember to switch to your live secret key in production!
                StripeConfiguration.ApiKey = Configuration.GetSection("Stripe").GetValue<string>("SecretKey");

                //charge the amount in card
                var myCharge = new ChargeCreateOptions();
                myCharge.Amount = (long)price * 100;
                myCharge.Currency = "EUR";
                myCharge.ReceiptEmail = email;
                myCharge.Description = "Sample Charge";
                myCharge.Source = stripeToken;
                myCharge.Capture = true;
                var chargeService = new ChargeService();
                Charge stripeCharge = await chargeService.CreateAsync(myCharge);

                if (stripeCharge.Status.Equals("succeeded"))
                {
                    result.Output = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return result;
            }
        }
    }
}
