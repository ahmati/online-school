using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Payment;
using OnlineSchool.Contract.Teachers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineSchool.Contract.SpotMeeting
{
    public class SpotMeetingModel : PurchasableItemModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get => StartDate.TimeOfDay; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public bool IsPublished { get; set; }
        public SpotMeetingStatus Status { get => GetSpotMeetingStatus(); }
        public DateTime AuthDate { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRecursiveSpotMeeting { get; set; }
        public string MeetingId { get; set; }
        public string MeetingTitle { get; set; }
        public string MeetingSipAddress { get; set; }
        public string MeetingPassword { get; set; }
        public bool IsLive { get; set; }
        public string Buttons { get; set; }
        public SpotMeetingTeacherModel Host { get; set; }
        public bool ReminderEmail { get; set; }
        public int AvailableSpots { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<SpotMeetingTeacherModel> SpotMeetingTeachers { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<SpotMeetingMaterialModel> SpotMeetingMaterials { get; set; }

        #region PurchasableItem
        public override int ItemId => Id;

        public override string ItemName => Title;

        public override double ItemPrice => Price;

        public override PurchasableItemType PurchasableItemType => PurchasableItemType.SpotMeeting;
        #endregion

        private SpotMeetingStatus GetSpotMeetingStatus()
        {
            var start = StartDate;
            var today = DateTime.Now;

            if (IsPublished == false)
            {
                if (today < start)
                    return SpotMeetingStatus.NotPublished;
                return SpotMeetingStatus.NotPublished_PastDue;
            }

            if (today < start || (today.Date == start.Date && (int)(today.TimeOfDay - start.TimeOfDay).TotalHours <= 0))
                return SpotMeetingStatus.Published;
            else if (today.Date == start.Date && (int)(today.TimeOfDay - start.TimeOfDay).TotalHours <= Duration)
                return SpotMeetingStatus.InProgress;
            else
                return SpotMeetingStatus.Finished;
        }
    }
}
