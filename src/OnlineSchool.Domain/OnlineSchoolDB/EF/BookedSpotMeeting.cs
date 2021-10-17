using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public class BookedSpotMeeting
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SpotMeetingId { get; set; }
        public double Price { get; set; }
        public DateTime AuthDate { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual SpotMeeting SpotMeeting { get; set; }
    }
}
