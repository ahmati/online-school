using Microsoft.AspNetCore.Http;
using OnlineSchool.Contract.Infrastructure.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Courses
{
    public class CreateCourseModel
    {
        public int TeacherSubjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public int AvailableSpots { get; set; }

        [DataType(DataType.Upload)]
        [Image]
        public IFormFile ImageFile { get; set; }
    }
}
