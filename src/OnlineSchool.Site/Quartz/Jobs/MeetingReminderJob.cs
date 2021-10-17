using ItalWebConsulting.Infrastructure.Comunication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Resources;
using OnlineSchool.Contract.Students;
using OnlineSchool.Core.Documents_.Service;
using OnlineSchool.Core.Students_.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Core.SpotMeeting_.Service;
using OnlineSchool.Core.Session_.Service;

namespace OnlineSchool.Site.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class MeetingReminderJob :IJob
    {
        public IStudentService _studentService { get; set; }
        public IEmailService _emailService { get; set; }
        public ISessionService _sessionService { get; set; }
        public ISpotMeetingService _spotMeetingService { get; set; }

        public MeetingReminderJob(IStudentService studentService, IEmailService emailService,ISessionService sessionService, ISpotMeetingService spotMeetingService)
        {
            _studentService = studentService;
            _emailService = emailService;
            _sessionService = sessionService;
            _spotMeetingService = spotMeetingService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await MeetingReminderEmail();
            await SpotMeetingReminderEmail();
        }

        private async Task MeetingReminderEmail()
        {
            try
            {
                var allSessions = await _sessionService.GetSessionsThatNeedRemindingAsync();

                foreach (var session in allSessions)
                {
                    var students = await _sessionService.GetStudentsByCourseIdAsync(session.CourseId);

                    var subject = EmailSharedResources.MeetingEmailsubject;
                    foreach (var student in students)
                    {
                        StringBuilder message = new StringBuilder(EmailSharedResources.MeetingEmailMessage);
                        message.Replace("user", student.Name);
                        message.Replace("topic", session.Topic);
                        message.Replace("meetingLink", $"localhost:44341/LiveSession/{session.Id}/Guest");

                        await _emailService.SendEmailAsync(student.Email, subject, message.ToString());
                    }

                    var teacherEmail = session.Course.TeacherSubject.Teacher.Email;
                    StringBuilder message1 = new StringBuilder(EmailSharedResources.MeetingEmailMessage);
                    message1.Replace("user", session.Course.TeacherSubject.Teacher.Name);
                    message1.Replace("topic", session.Topic);
                    message1.Replace("meetingLink", $"localhost:44341/LiveSession/{session.Id}/Guest");
                    await _emailService.SendEmailAsync(teacherEmail,subject , message1.ToString());
                    await _sessionService.UpdateReminderEmailAsync(session.Id);

                }
            }
            catch (Exception e)
            {
            }
        }

        private async Task  SpotMeetingReminderEmail()
        {
            try
            {
                var spotMeetings = await _spotMeetingService.GetSpotMeetingsThatNeedRemindingAsync();

                foreach (var meeting in spotMeetings)
                {
                        var spotMeetingUsers = await _spotMeetingService.GetUsersBySpotMeetingAsync(meeting.Id);
                        foreach (var user in spotMeetingUsers)
                        {
                            var subject = EmailSharedResources.MeetingEmailsubject;
                            StringBuilder message1 = new StringBuilder(EmailSharedResources.MeetingEmailMessage);
                            message1.Replace("user", user.User.Name);
                            message1.Replace("topic", meeting.Title);
                            message1.Replace("meetingLink", $"localhost:44341/SpotMeeting/{meeting.MeetingId}/Guest");

                            await _emailService.SendEmailAsync(user.User.Email, subject, message1.ToString());
                        }

                        await _spotMeetingService.UpdateReminderEmailAsync(meeting.Id);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
