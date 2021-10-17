using ItalWebConsulting.Infrastructure.BusinessLogic;
using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.Comunication;
using Microsoft.Extensions.Logging;
using NLog;
using OnlineSchool.Contract.Calendar;
using OnlineSchool.Contract.Resources;
using OnlineSchool.Contract.SpotMeeting;
using OnlineSchool.Core.SpotMeeting_.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.IcsCalendar.Service
{
    public class IcsCalendarService : CoreBase, IIcsCalendarService
    {
        public IEmailService EmailService { get; set; }
        public async Task<bool> SendIcsFile(IcsCalendarModel calendarModel)
        {
            try
            {
                if (calendarModel.Email == null)
                    throw new ArgumentNullException(nameof(calendarModel.Email));

                StringBuilder emailBody =new StringBuilder( EmailSharedResources.CalendarEmail);
                emailBody = emailBody.Replace("name", calendarModel.UserName.ToUpper());

                MailMessage msg = new MailMessage("supporto@infowebgroup.it", calendarModel.Email);
                msg.Subject = "OnlineSchool Calendar";
                msg.Body = emailBody.ToString();
                
                DateTime StartTime = calendarModel.StartDate + calendarModel.StartTime;
                DateTime EndTime = StartTime.AddHours(calendarModel.Duration);

                string Summary = calendarModel.Title;
                string Description = calendarModel.Description;

                //create a new stringbuilder instance
                StringBuilder sb = new StringBuilder();

                //start the calendar item
                sb.AppendLine("BEGIN:VCALENDAR");
                sb.AppendLine("VERSION:2.0");
                sb.AppendLine("CALSCALE:GREGORIAN");
                sb.AppendLine("METHOD:PUBLISH");

                //create a time zone if needed, TZID to be used in the event itself
                sb.AppendLine("BEGIN:VTIMEZONE");
                sb.AppendLine("TZID:Europe/Italia");
                sb.AppendLine("BEGIN:STANDARD");
                sb.AppendLine("TZOFFSETTO:+0100");
                sb.AppendLine("TZOFFSETFROM:+0100");
                sb.AppendLine("END:STANDARD");
                sb.AppendLine("END:VTIMEZONE");

                //add the event
                sb.AppendLine("BEGIN:VEVENT");

                sb.AppendLine("DTSTART:" + StartTime.ToString("yyyyMMddTHHmm00"));
                sb.AppendLine("DTEND:" + EndTime.ToString("yyyyMMddTHHmm00"));

                sb.AppendLine("BEGIN:VALARM");
                sb.AppendLine("TRIGGER:-PT15M");
                sb.AppendLine("ACTION:DISPLAY");
                sb.AppendLine("DESCRIPTION:Reminder");
                sb.AppendLine("END:VALARM");

                sb.AppendLine("SUMMARY:" + Summary + "");
                sb.AppendLine("DESCRIPTION:" + Description + "");
                sb.AppendLine("PRIORITY:3");
                sb.AppendLine("END:VEVENT");

                //end calendar item
                sb.AppendLine("END:VCALENDAR");

                //create a string from the stringbuilder
                string CalendarItem = sb.ToString();
                byte[] byteArray = Encoding.ASCII.GetBytes(CalendarItem);
                //MemoryStream stream = new MemoryStream(byteArray);
                using(var stream =  new MemoryStream(byteArray))
                {
                    Attachment attach = new Attachment(stream, calendarModel.Title + ".ics");

                    msg.Attachments.Add(attach);

                    var result = await EmailService.SendCalendarEmailAsync(msg);
                    return result;
                } 

            }
            catch (Exception ex)
            {
                    throw ex;
            }
            
        }

    }
}
