using Microsoft.AspNetCore.Http;
using OnlineSchool.Contract.Infrastructure.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Teachers
{
    public class CreateTeacherModel
    {
        
        public string Name { get; set; }

        public string Surname { get; set; }

        [RegularExpression("[M|F]")]
        public char Gender { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        [DataType(DataType.Upload)]
        [Image]
        public IFormFile ImageFile { get; set; }
    }
}
