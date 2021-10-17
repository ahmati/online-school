using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Contacts
{
   public class ContactRequestModel
    {
        public short Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
