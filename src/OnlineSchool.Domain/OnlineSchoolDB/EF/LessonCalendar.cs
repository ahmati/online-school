using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class LessonCalendar
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public string AspNetUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual Lesson Lesson { get; set; }
    }
}
