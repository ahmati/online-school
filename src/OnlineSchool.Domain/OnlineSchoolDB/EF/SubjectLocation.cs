using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class SubjectLocation
    {
        public int Id { get; set; }
        public int SubjectTimeId { get; set; }
        public byte? SubjectLocationTypeId { get; set; }
        public string Location { get; set; }

        public virtual LocationType SubjectLocationType { get; set; }
    }
}
