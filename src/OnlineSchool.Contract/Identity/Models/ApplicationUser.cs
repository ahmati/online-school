using Microsoft.AspNetCore.Identity;
using OnlineSchool.Contract.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public char Gender { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
