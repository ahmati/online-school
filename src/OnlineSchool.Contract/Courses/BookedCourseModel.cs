using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Courses
{
    public class BookedCourseModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public decimal Price { get; set; }
        public DateTime AuthDate { get; set; }
        public int CourseId { get; set; }
        public CourseStatus Status { get => GetCourseStatus(); }
        public bool MeetingEmailReminder { get; set; }


        public CourseModel Course { get; set; }

        private CourseStatus GetCourseStatus()
        {
            var start = Course.StartDate.Date;
            var end = Course.EndDate.Date;
            var today = DateTime.Now.Date;

            if (Course.IsPublished == false)
            {
                if (today < start)
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
