using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Webex
{
    public class CreateMeetingModel
    {
        public string title { get; set; }

        /// <summary> ISO 8601 format </summary>
        public string start { get; set; }

        /// <summary> ISO 8601 format </summary>
        public string end { get; set; }
    }
}
