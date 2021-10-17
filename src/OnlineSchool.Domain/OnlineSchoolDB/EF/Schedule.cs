using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TeacherId { get; set; }
        public int? SubjectId { get; set; }
        public int? ColorId { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public int? RecurrenceId { get; set; }
        public string RecurrenceException { get; set; }
        public bool? IsAllDay { get; set; }
        public string StartTimezone { get; set; }

        public virtual Color Color { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
