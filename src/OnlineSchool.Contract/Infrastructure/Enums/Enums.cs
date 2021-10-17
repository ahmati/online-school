using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnlineSchool.Contract.Infrastructure.Enums
{
    public enum Roles
    {
        [Description("Agente")]
        Agent,
        [Description("Student")]
        Student,
        [Description("Teacher")]
        Teacher,
        [Description("Admin")]
        Admin
    }

    public enum Genders
    {
        [Description("M")]
        Male,
        [Description("F")]
        Female
    }

    public enum SubjectCategories
    {
        Subject = 1,
        Event = 2
    }
}
