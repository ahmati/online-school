using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Contract.Subject
{
    public class CreateSubjectModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Password)]
        public double Price { get; set; }

        public string Color { get; set; } = "#FFFFFF";
    }
}
