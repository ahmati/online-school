using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Caching
{
    public class CacheContainer
    {
        public object Value { get; set; }
        public Type ValueType { get => Value != null ? Value.GetType() : null; }
        public DateTime CachedDate { get; set; }
    }
}
