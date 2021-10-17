
namespace OnlineSchool.Contract.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Kendo.Mvc.UI;
    public class SchedulerModel : Kendo.Mvc.UI.ISchedulerEvent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        private DateTime StartDate;
        public DateTime Start
        {
            get
            {
                return StartDate;
            }
            set
            {
                StartDate = value.ToUniversalTime();
            }
        }
        public string StartTimezone { get; set; }
        private DateTime EndDate;
        public DateTime End
        {
            get
            {
                return EndDate;
            }
            set
            {
                EndDate = value.ToUniversalTime();
            }
        }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public int? RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }
        [Display(Name = "Teacher Name")]
        public int? TeacherId { get; set; }
        [Display(Name = "Subject Name")]
        public int? SubjectId { get; set; }
        public int? ColorId { get; set; }
        [Display(Name = "Teacher Name")]
        public string TeacherName { get; set; }
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

       
    }
}
