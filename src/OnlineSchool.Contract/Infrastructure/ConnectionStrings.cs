using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Infrastructure
{
    public class ConnectionStrings : IConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class ConnectionStringDev : ConnectionStrings
    {

    }

    public interface IConnectionStrings
    {
        string DefaultConnection { get; }
    }
}
