using OnlineSchool.Contract.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Students
{
    public class StudentModel : UserModel
    {
        public DateTime AuthDate { get; set; }
    }
}
