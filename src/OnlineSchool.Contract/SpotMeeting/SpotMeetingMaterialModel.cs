using OnlineSchool.Contract.Material;
using System;

namespace OnlineSchool.Contract.SpotMeeting
{
    public class SpotMeetingMaterialModel
    {
        public int Id { get; set; }
        public int SpotMeetingId { get; set; }
        public int MaterialId { get; set; }
        public DateTime AuthDate { get; set; }

        public SpotMeetingModel SpotMeeting { get; set; }
        public MaterialModel Material { get; set; }
    }
}