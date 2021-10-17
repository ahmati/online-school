using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Identity.Models
{
   public class RoleUserModel
    {
        public int RoleId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
