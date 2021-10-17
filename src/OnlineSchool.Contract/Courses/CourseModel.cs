using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Payment;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.TeacherSubject;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineSchool.Contract.Courses
{
    public class CourseModel : PurchasableItemModel
    {
        public CourseModel()
        {
            Sessions = new HashSet<SessionModel>();
        }

        public int Id { get; set; }
        public int TeacherSubjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public bool IsPublished { get; set; }
        public CourseStatus Status { get => GetCourseStatus(); }
        public DateTime AuthDate { get; set; }
        public string ImagePath { get; set; }
        public bool IsLive { get; set; }
        public string Buttons { get; set; }
        public int AvailableSpots { get; set; }
        public TeacherSubjectModel TeacherSubject { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<SessionModel> Sessions { get; set; }

        #region PurchasableItem
        public override int ItemId => Id;

        public override string ItemName => TeacherSubject?.Subject?.Name;

        public override double ItemPrice => Price;

        public override PurchasableItemType PurchasableItemType => PurchasableItemType.Course;
        #endregion

        private CourseStatus GetCourseStatus()
        {
            var start = StartDate.Date;
            var end = EndDate.Date;
            var today = DateTime.Now.Date;

            if (IsPublished == false)
            {
                if(today < start)
                    return CourseStatus.NotPublished;
                return CourseStatus.NotPublished_PastDue;
            }

            if (today < start)
                return CourseStatus.Published;
            else if (today >= start && today <= end)
                return CourseStatus.InProgress;
            else
                return CourseStatus.Finished;
        }
    }
}
