using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnlineSchool.Contract.Contacts
{
    public enum PurchasableItemType
    {
        [Description("Course")]
        Course = 0,

        [Description("Meeting")]
        SpotMeeting = 1
    }
}
