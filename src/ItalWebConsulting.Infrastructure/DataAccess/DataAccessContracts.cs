using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ItalWebConsulting.Infrastructure.DataAccess
{
    public class GetSqlSelectByColumnnsOutput
    {
        public string SqlSelectWithWhere { get; set; }
        public IEnumerable<SqlParameter> Parameters { get; set; }
    }
}
