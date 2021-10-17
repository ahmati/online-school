using OnlineSchool.Contract.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Identity.Models
{
    public class ResetPassword
    {
        [Display(Name = nameof(SharedResources.Email), ResourceType = typeof(SharedResources))]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(SharedResources.Password), ResourceType = typeof(SharedResources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(SharedResources.ConfirmPassword), ResourceType = typeof(SharedResources))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
