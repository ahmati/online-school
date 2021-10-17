using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
   public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public int TeacherId { get; set; }

    }
}
