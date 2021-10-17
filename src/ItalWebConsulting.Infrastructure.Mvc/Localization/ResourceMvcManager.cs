using ItalWebConsulting.Infrastructure.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Mvc.Localization
{
    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-3.1
    public class ResourceMvcManager: ResourceManager, IResourceMvcManager
    {
        public IHtmlLocalizerFactory HtmlLocalizerFactory { get; set; }
        public IHtmlLocalizer GetHtmlLocalizer<TView>(Assembly resourceAssembly)
        {
            var ctResource = "Views." + typeof(TView).Name;
            return HtmlLocalizerFactory.Create(ctResource, resourceAssembly.GetName().Name);
        }
        public IHtmlLocalizer GetHtmlLocalizer(string resourceName, Assembly resourceAssembly)
        {
            return HtmlLocalizerFactory.Create(resourceName, resourceAssembly.GetName().Name);
        }
        public IHtmlLocalizer GetHtmlLocalizer(Type view, Assembly resourceAssembly)
        {
            //var ctResource = "Views." + view.Name;
            var ctResource = view.Name.Replace('_', '.');
            ctResource = ctResource.Replace("..", "._");
            return HtmlLocalizerFactory.Create(ctResource, resourceAssembly.GetName().Name);
        }
    }

    public interface IResourceMvcManager: IResourceManager
    {
        IHtmlLocalizer GetHtmlLocalizer<TView>(Assembly resourceAssembly);
        IHtmlLocalizer GetHtmlLocalizer(string resourceName, Assembly resourceAssembly);
        IHtmlLocalizer GetHtmlLocalizer(Type view, Assembly resourceAssembly);
    }
}
