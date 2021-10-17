using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Common
{
    public class CodeValue<TCode, TValue>
    {
        public TCode Code { get; set; }
        public TValue Value { get; set; }
    }

    public class StrCodeObjValue: CodeValue<string, object>
    {
    }
}
