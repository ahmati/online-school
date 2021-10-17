using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class BookedCourse 
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public double Price { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
