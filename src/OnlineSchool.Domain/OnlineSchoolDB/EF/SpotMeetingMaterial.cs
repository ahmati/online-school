using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public class SpotMeetingMaterial
    {
        public virtual int Id { get; set; }
        public virtual int SpotMeetingId { get; set; }
        public virtual int MaterialId { get; set; }
        public virtual DateTime AuthDate { get; set; }

        public virtual SpotMeeting SpotMeeting { get; set; }
        public virtual Material Material { get; set; }
    }
}
