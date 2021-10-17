using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnlineSchool.Contract.Courses
{
    public enum CourseStatus
    {
        [Description("Not published")]
        NotPublished = 0,

        [Description("Not published (due date has passed)")]
        NotPublished_PastDue = 1,

        [Description("Published")]
        Published = 2,

        [Description("In progress")]
        InProgress = 3,

        [Description("Finished")]
        Finished = 4,
    }
}
