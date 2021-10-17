using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class SubjectCategory
    {
        public SubjectCategory()
        {
            Subjects = new HashSet<Subject>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
