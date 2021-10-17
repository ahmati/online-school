using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.SpotMeeting
{
    public class BookedSpotMeetingModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SpotMeetingId { get; set; }
        public double Price { get; set; }
        public DateTime AuthDate { get; set; }
        public SpotMeetingStatus Status { get => GetSpotMeetingStatus(); }

        public ApplicationUser User { get; set; }
        public SpotMeetingModel SpotMeeting { get; set; }

        private SpotMeetingStatus GetSpotMeetingStatus()
        {
            var start = SpotMeeting.StartDate.Date;
            var today = DateTime.Now.Date;

            if (SpotMeeting.IsPublished == false)
            {
                if (today < start)
                    return SpotMeetingStatus.NotPublished;
                return SpotMeetingStatus.NotPublished_PastDue;
            }

            if (today < start)
                return SpotMeetingStatus.Published;
            else if (today >= start && (int)(today - start).TotalHours <= SpotMeeting.Duration)
                return SpotMeetingStatus.InProgress;
            else
                return SpotMeetingStatus.Finished;
        }
    }
}
