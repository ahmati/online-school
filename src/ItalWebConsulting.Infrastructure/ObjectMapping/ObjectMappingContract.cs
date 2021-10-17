using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.ObjectMapping
{
    public class ObjectMapping
    {
        public Type Source { get; set; }
        public Type Destination { get; set; }
        public bool MapBothDirections { get; set; }
    }
}
