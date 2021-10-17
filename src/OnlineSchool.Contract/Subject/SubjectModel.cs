using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace OnlineSchool.Contract.Subject
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public DateTime AuthDate { get; set; }
        public byte CategoryId { get; set; }

    }
}
