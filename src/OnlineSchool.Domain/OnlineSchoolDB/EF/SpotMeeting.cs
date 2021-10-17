using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
    public class SpotMeeting
    {
        public SpotMeeting()
        {
            SpotMeetingMaterials = new HashSet<SpotMeetingMaterial>();
            SpotMeetingTeachers = new HashSet<SpotMeetingTeacher>();
            BookedSpotMeetings = new HashSet<BookedSpotMeeting>();
        }

        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual int Duration { get; set; }
        public virtual double Price { get; set; }
        public virtual bool IsPublished { get; set; }
        public virtual DateTime AuthDate { get; set; }
        public virtual bool IsRecursiveSpotMeeting { get; set; }
        public virtual string MeetingId { get; set; }
        public virtual string MeetingTitle { get; set; }
        public virtual string MeetingSipAddress { get; set; }
        public virtual string MeetingPassword { get; set; }
        public bool ReminderEmail { get; set; }
        public virtual int AvailableSpots { get; set; }

        public virtual ICollection<SpotMeetingTeacher> SpotMeetingTeachers { get; set; }
        public virtual ICollection<SpotMeetingMaterial> SpotMeetingMaterials { get; set; }
        public virtual ICollection<BookedSpotMeeting> BookedSpotMeetings { get; set; }
    }
}
