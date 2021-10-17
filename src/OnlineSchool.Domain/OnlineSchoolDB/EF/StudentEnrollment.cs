using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class StudentEnrollment
    {
        public int Id { get; set; }
        public int? TeacherSubjectId { get; set; }
        public int? StudentId { get; set; }

        public virtual Student Student { get; set; }
    }
}
