using ItalWebConsulting.Infrastructure.Localization;
using ItalWebConsulting.Infrastructure.Mvc.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Localization;
using OnlineSchool.Contract.Globalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Mvc.Extension
{
    public static class HtmlLocalizerHelper
    {
        public static IHtmlLocalizer SharedViewLocalizer(HttpContext Context)
        {
            var rm = Context.GetService<IResourceMvcManager>();
            return rm.GetHtmlLocalizer("SharedResources", typeof(CultureNaming).Assembly);
        }

        public static IHtmlLocalizer ViewLocalizer(HttpContext Context, Type viewType)
        {
            var rm = Context.GetService<IResourceMvcManager>();
            return rm.GetHtmlLocalizer(viewType, typeof(CultureNaming).Assembly);
        }
    }
}
