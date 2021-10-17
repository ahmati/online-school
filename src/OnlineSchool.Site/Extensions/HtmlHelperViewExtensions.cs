using ItalWebConsulting.Infrastructure.Mvc.Extension;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Extensions
{
    public static class HtmlHelperViewExtensions
    {
        public static IHtmlContent IncludeVersionedJs(this IHtmlHelper helper, HttpContext context, string filePath)
        {
            filePath = filePath.Replace("~", "");
            return new HtmlString(CreateScriptRef(filePath, GetVersion(context)));
        }
        static string CreateScriptRef(string filePath, string version)
        {
            return string.Concat("<script src='", filePath, version, "' type='text/javascript'></script>");
        }
        static string GetVersion(HttpContext context)
        {
            var cache = context.GetService<ItalWebConsulting.Infrastructure.Caching.ICacheManager>();
            return $"?v={cache}";
        }
    }
}
