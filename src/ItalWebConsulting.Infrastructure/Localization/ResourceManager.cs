using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using System.Collections.Concurrent;
using System.IO;

using System.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace ItalWebConsulting.Infrastructure.Localization
{
    //https://github.com/aspnet/Localization/blob/60a3d57a3e8b8b6d69232815a9596b5a495a7d0b/src/Microsoft.Extensions.Localization/ResourceManagerStringLocalizerFactory.cs#L181
    public class ResourceManager : IResourceManager
    {
        public IStringLocalizerFactory StringLocalizerFactory { get; set; }
        public IStringLocalizer GetStringLocalizer<TController>(Assembly resourceAssembly)
        {
            var ctResource = "Controllers." + typeof(TController).Name;
            return StringLocalizerFactory.Create(ctResource, resourceAssembly.GetName().Name);
        }

        public IStringLocalizer GetStringLocalizer(string resourceName, Assembly resourceAssembly)
        {
            return StringLocalizerFactory.Create(resourceName, resourceAssembly.GetName().Name);
        }

        //public IHtmlLocalizer GetHtmlLocalizer<TView>(Assembly resourceAssembly)
        //{
        //    var ctResource = "Views." + typeof(TView).Name;
        //    return HtmlLocalizerFactory.Create(ctResource, resourceAssembly.GetName().Name);
        //}

        //public IHtmlLocalizer GetHtmlLocalizer(string resourceName, Assembly resourceAssembly)
        //{
        //    return HtmlLocalizerFactory.Create(resourceName, resourceAssembly.GetName().Name);
        //}
    }
    public interface IResourceManager
    {
        IStringLocalizer GetStringLocalizer<TController>(Assembly resourceAssembly);
        IStringLocalizer GetStringLocalizer(string resourceName, Assembly resourceAssembly);
        //IHtmlLocalizer GetHtmlLocalizer<TView>(Assembly resourceAssembly);
        //IHtmlLocalizer GetHtmlLocalizer(string resourceName, Assembly resourceAssembly);
    }

}
