using Microsoft.Extensions.Configuration;
using OnlineSchool.Contract.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Payment_.Service
{
    public interface IPaymentService
    {
        Task<ResponseBase<bool>> PayIt(string stripeToken, decimal price, string name, string email);
    }
}
