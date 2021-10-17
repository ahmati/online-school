using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
   public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int SubjectId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Order Order { get; set; }
    }
}
