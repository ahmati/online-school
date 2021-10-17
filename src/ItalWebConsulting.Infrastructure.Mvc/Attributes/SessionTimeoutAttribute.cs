using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Mvc.Attributes
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session == null ||
                     !context.HttpContext.Session.TryGetValue("CurrentLoggedUser", out byte[] val))
            {
                context.Result =
                    new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "Login"
                    }));
            }
            base.OnActionExecuting(context);
        }
    }
}
