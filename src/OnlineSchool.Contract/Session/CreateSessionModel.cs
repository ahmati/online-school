using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Session
{
    public class CreateSessionModel
    {
        public int CourseId { get; set; }
        public string Topic { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
