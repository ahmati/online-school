using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public class SpotMeetingTeacher
    {
        public virtual int Id { get; set; }
        public virtual int SpotMeetingId { get; set; }
        public virtual int TeacherId { get; set; }
        public virtual DateTime AuthDate { get; set; }
        public virtual bool IsHost { get; set; }

        public virtual SpotMeeting SpotMeeting { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
