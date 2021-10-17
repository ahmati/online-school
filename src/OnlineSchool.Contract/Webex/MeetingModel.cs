using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Webex
{
    public class MeetingModel
    {
        public string id { get; set; }
        public string meetingNumber { get; set; }
        public string title { get; set; }
        public string password { get; set; }
        public string state { get; set; }
        public string timezone { get; set; }
        /// <summary> ISO 8601 format </summary>
        public string start { get; set; }
        /// <summary> ISO 8601 format </summary>
        public string end { get; set; }
        public string webLink { get; set; }
        public string sipAddress { get; set; }
        public bool enabledAutoRecordMeeting { get; set; }
        public bool enabledJoinBeforeHost { get; set; }
        public int joinBeforeHostMinutes { get; set; }
    }

    public class WebexMeetingInviteeModel
    {
        public string email { get; set; }
        public string displayName { get; set; }
        public bool coHost { get; set; }
    }
}
