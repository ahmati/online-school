using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnlineSchool.Contract.Webex
{
    public enum MeetingState
    {
        [Description("active")]
        Active,
        [Description("inProgress")]
        InProgress
    }
}
