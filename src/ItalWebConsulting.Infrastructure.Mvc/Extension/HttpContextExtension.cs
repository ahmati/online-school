using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Mvc.Extension
{
    public static class HttpContextExtension
    {
        public static T GetService<T>(this HttpContext context)
        {
            return (T)context.RequestServices.GetService(typeof(T));
        }
    }
}
