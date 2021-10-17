using Microsoft.AspNetCore.Http;
using OnlineSchool.Contract.Infrastructure.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.SpotMeeting
{
    public class CreateSpotMeetingModel
    {
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRecursiveSpotMeeting { get; set; }
        public int HostId { get; set; }
        public int AvailableSpots { get; set; }

        [DataType(DataType.Upload)]
        [Image]
        public IFormFile ImageFile { get; set; }
    }
}
