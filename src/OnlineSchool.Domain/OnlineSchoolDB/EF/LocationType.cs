using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class LocationType
    {
        public LocationType()
        {
            SubjectLocations = new HashSet<SubjectLocation>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubjectLocation> SubjectLocations { get; set; }
    }
}
