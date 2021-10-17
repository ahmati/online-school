using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineSchool.Domain.OnlineSchoolDB.EF
{
   public class Order
    {
        public int OrderId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }   
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsFree { get; set; }
        public string UserId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
