using System;
using System.Collections.Generic;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public partial class Color
    {
        public Color()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
