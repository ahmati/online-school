using ItalWebConsulting.Infrastructure.Extension;
using OnlineSchool.Contract.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Utility
{
    public static class Helpers
    {
        public static bool IsGenderValid(char gender)
        {
            if (char.IsWhiteSpace(gender) || !char.IsLetter(gender))
                return false;

            var genders = new List<char>();
            foreach (var g in Enum.GetValues(typeof(Genders)))
            {
                genders.Add(((Genders)g).GetDescription()[0]);
            }

            if (genders.Contains(gender))
                return true;
            return false;
        }
    }
}
