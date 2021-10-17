using OnlineSchool.Contract.Courses;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineSchool.Contract.Session
{
    public class SessionModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string MeetingId { get; set; }
        public string MeetingTitle { get; set; }
        public string MeetingSipAddress { get; set; }
        public string MeetingPassword { get; set; }
        public DateTime AuthDate { get; set; }
        public bool ReminderEmail { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CourseModel Course { get; set; }
    }
}
