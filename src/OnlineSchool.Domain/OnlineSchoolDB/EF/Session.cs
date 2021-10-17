using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public class Session
    {
        public virtual int Id { get; set; }
        public virtual int CourseId { get; set; }
        public virtual string Topic { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual TimeSpan StartTime { get; set; }
        public virtual TimeSpan EndTime { get; set; }
        public virtual string MeetingId { get; set; }
        public virtual string MeetingTitle { get; set; }
        public virtual string MeetingSipAddress { get; set; }
        public virtual string MeetingPassword { get; set; }
        public virtual DateTime AuthDate { get; set; }
        public bool ReminderEmail { get; set; }

        public virtual Course Course { get; set; }
    }
}
