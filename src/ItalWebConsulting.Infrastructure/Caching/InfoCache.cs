using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Caching
{
    public class InfoCache
    {
        public string Key { get; set; }
        public Type ValueType { get; set; }
        public DateTime? CachedDate { get; set; }
    }
}
