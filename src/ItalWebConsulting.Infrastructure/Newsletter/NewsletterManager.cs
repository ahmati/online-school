//using System;
//using System.Collections.Generic;
//using System.Text;

//using mailinblue;
//namespace InfoWeb.Infrastructure.Newsletter
//{
//    public class NewsletterManager
//    {
//        public  NewsletterOutput Send(NewsletterInput input)
//        {
//            API sendinBlue = new mailinblue.API(input.ApiKey); //add your api key here 
//            Dictionary<string, Object> data = new Dictionary<string, Object>();
//            data.Add("to", input.To);
//            data.Add("from", input.From);
//            data.Add("subject", input.Subject);
//            data.Add("html", input.Html);
//            data.Add("attachment", input.Attachment);
//            dynamic sendEmail = sendinBlue.send_email(data);
//            NewsletterOutput output = new NewsletterOutput()
//            {
//                Status = (string)sendEmail.code,
//                Message = (string)sendEmail.message
//            };
//            return output;
           
//        }
//    }
//}
