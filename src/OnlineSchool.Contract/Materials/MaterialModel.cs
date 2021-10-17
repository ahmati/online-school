using Microsoft.AspNetCore.Http;
using OnlineSchool.Contract.Infrastructure.FileUpload;
using OnlineSchool.Contract.Teachers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Material
{
    public class MaterialModel : FileBase
    {
        public int Id { get; set; } 
        public DateTime AuthDate { get; set; }
    }
}
