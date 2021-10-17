using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Calendar
{
    public class IcsCalendarModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get ; set; }
        public string Title { get; set; }
        public int Duration { get; set; }

    }
}
