using OnlineSchool.Contract.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Identity.Models
{
    public class ForgotPassword
    {
        [Display(Name = nameof(SharedResources.Email), ResourceType = typeof(SharedResources))]
        public string Email { get; set; }
    }
}
