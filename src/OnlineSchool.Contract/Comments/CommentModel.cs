using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Comments
{
   public class CommentModel
    {
        public int CommentId { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
    }
}
