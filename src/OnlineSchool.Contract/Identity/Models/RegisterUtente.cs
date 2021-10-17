using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineSchool.Contract.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Text;

namespace OnlineSchool.Contract.Identity.Models
{
    public class RegisterUtente
    {
      
        [Display(Name = nameof(SharedResources.Name), ResourceType = typeof(SharedResources))]
        public string Name { get; set; }

        [Display(Name = nameof(SharedResources.Surname), ResourceType = typeof(SharedResources))]
        public string Surname { get; set; }

        [RegularExpression("[M|F]")]
        [Display(Name = nameof(SharedResources.Gender), ResourceType = typeof(SharedResources))]
        public char Gender { get; set; }

        [Display(Name = nameof(SharedResources.Email), ResourceType = typeof(SharedResources))]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(SharedResources.Password), ResourceType = typeof(SharedResources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(SharedResources.ConfirmPassword), ResourceType = typeof(SharedResources))]
        public string ConfirmPassword { get; set; }

    }
}
