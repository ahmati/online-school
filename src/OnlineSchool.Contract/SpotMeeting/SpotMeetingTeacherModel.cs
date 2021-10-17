using OnlineSchool.Contract.Teachers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.SpotMeeting
{
    public class SpotMeetingTeacherModel
    {
        public int Id { get; set; }
        public int SpotMeetingId { get; set; }
        public int TeacherId { get; set; }
        public DateTime AuthDate { get; set; }
        public bool IsHost { get; set; }

        public SpotMeetingModel SpotMeeting { get; set; }
        public TeacherModel Teacher { get; set; }
    }
}
