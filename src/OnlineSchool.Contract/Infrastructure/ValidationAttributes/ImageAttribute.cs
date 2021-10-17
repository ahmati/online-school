using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace OnlineSchool.Contract.Infrastructure.ValidationAttributes
{
    public class ImageAttribute : ValidationAttribute
    {
        private readonly string[] _extensions = { ".jpg", ".jpeg", ".png" };
        public ImageAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage(extension));
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string extension)
        {
            return $"The selected file format ({extension}) is not allowed!";
        }
    }
}
