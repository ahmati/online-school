using AutoMapper;
using ItalWebConsulting.Infrastructure.Caching;
using ItalWebConsulting.Infrastructure.Composition;
using ItalWebConsulting.Infrastructure.Localization;
using ItalWebConsulting.Infrastructure.Mvc.Caching;
using ItalWebConsulting.Infrastructure.Mvc.Localization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.Mvc
{
    public abstract class ControllerBase : ControllerBase<ControllerBase>
    {

    }

    //https://andrewlock.net/adding-localisation-to-an-asp-net-core-application/
    public abstract class ControllerBase<TController> : Controller where TController : class
    {
        private const string FUNCTION_START = "Funzione {0} - Start";
        private const string FUNCTION_STOP = "Funzione {0} - Stop";
        private const string USER_ERROR_MESSAGE = "Errore imprevisto contattare il supporto tecnico";

        public ILogger<TController> Logger { get; set; }
        public Lazy<IMapper> Mapper { get; set; }
        public Lazy<IServiceProvider> ServiceProvider { get; set; }
        public Lazy<ICacheManager> CacheApplicationManager { get; set; }
        public Lazy<IMvcInHttpSessionCache> CacheSessionManager { get; set; }
        public Lazy<IWebHostEnvironment> WebHostEnvironment { get; set; }
        public Lazy<IHttpContextAccessor> HttpContextAccessor { get; set; }
        public IResourceMvcManager ResourceManager { get; set; }

        //IMvcInHttpSessionCache cacheSessionManager;
        //protected IStringLocalizer CurrentLocalizer { get => ResourceManager.GetStringLocalizer<TController>(typeof(AgencyModel).Assembly); }
        //protected IStringLocalizer SharedLocalizer { get => ResourceManager.GetStringLocalizer("SharedResources", typeof(AgencyModel).Assembly); }

        public HttpRequest GetRequest()
        {
            return HttpContextAccessor.Value.HttpContext.Request;
        }

        public string GetUrlRoot()
        {
            var request = GetRequest();
            return string.Format("{0}://{1}/", request.Scheme, request.Host);
        }

        public ClaimsPrincipal GetUser()
        {
            return HttpContextAccessor.Value.HttpContext.User;
        }

        //{
        //    get
        //    {
        //        if (!cacheSessionManager.IsInitializedCache())
        //            cacheSessionManager.InitializeCache(HttpContext.Session);

        //        return cacheSessionManager;
        //    }
        //    set
        //    {
        //        cacheSessionManager = value;
        //    }
        //}
        public ITempDataDictionary CachePageManager { get => this.TempData; }
        public Lazy<IUrlHelper> UrlHelper { get; set; }
        public Lazy<IConfigurationManager> configurationManager { get; set; }
       
        public T GetService<T>()
        {
            return (T)(ServiceProvider.Value.GetService(typeof(T)));
        }

        protected async Task<ActionResult> ExecuteForKendoDataSourceCtrl<TOutput>(Func<Task<IEnumerable<TOutput>>> func, DataSourceRequest request, bool addModelStateToResponse = false)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                Logger.LogDebug(FUNCTION_START, func.Method.Name);

                var result = await func.Invoke();
                if (result == null)
                {
                    Logger.LogError("Il metodo " + func.Method.Name + " ha restituito un risultato nullo");
                    return AddModelError();
                }
                if (addModelStateToResponse)
                {
                    var lst = await result.ToDataSourceResultAsync(request, ModelState);
                    return await Task.FromResult(Json(lst));
                }
                
                var lstas = await result.ToDataSourceResultAsync(request);
                return await Task.FromResult(Json(lstas));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, " - Errore nel metodo " + func.Method.Name);
                return AddModelError();
            }
            finally
            {
               Logger.LogDebug(FUNCTION_STOP, func.Method.Name);
            }
        }

        protected async Task<ActionResult> ExecuteForOk(Func<Task<bool>> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                Logger.LogDebug(FUNCTION_START, func.Method.Name);

                var result = await func.Invoke();
                if (!result)
                {
                    Logger.LogError("Il metodo " + func.Method.Name + " ha restituito un risultato false");
                }

                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, " - Errore nel metodo " + func.Method.Name);
                return AddModelError();
            }
            finally
            {
                Logger.LogDebug(FUNCTION_STOP, func.Method.Name);
            }
        }

        protected async Task<ActionResult> ExecuteForView(Func<Task<bool>> func, string viewName = null)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            try
            {
                Logger.LogDebug(FUNCTION_START, func.Method.Name);

                var result = await func.Invoke();
                if (!result)
                {
                    Logger.LogError("Il metodo " + func.Method.Name + " ha restituito un risultato false");
                }
                if(viewName != null)
                    return await Task.FromResult(View(viewName));

                return await Task.FromResult(View());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, " - Errore nel metodo " + func.Method.Name);
                return AddModelError();
            }
            finally
            {
                Logger.LogDebug(FUNCTION_STOP, func.Method.Name);
            }
        }


        private JsonResult AddModelError()
        {
            return AddModelError(USER_ERROR_MESSAGE);
        }
        private JsonResult AddModelError(string message)
        {
            ModelState.AddModelError("Error", message);
            var resultErr = ModelState.ToDataSourceResult();
            return Json(resultErr);
        }
        /// <summary>
        /// Wrapper di chiamata ad un metodo interno al Controller. Il metodo da chiamare restituisce un contract di tipo TOutput.
        /// </summary>
        /// <typeparam name="TOutput">Contratto di output. Risultato della chiamata al componente Core</typeparam>
        /// <param name="func">Lambda di esecusione del metodo core</param>
        /// <param name="logStartStop">Se è true vengono generate due righe di debug, all'inizio e alla fine del metodo Scalable</param>
        /// <returns></returns>
        //protected TOutput ExecuteFunction<TOutput>(Func<TOutput> func, bool logStartStop = false)
        //      {
        //          var result = default(TOutput);
        //          var hasError= false;
        //          if (func == null)
        //              throw new ArgumentNullException(nameof(func));

        //          //func.Method.f
        //          try
        //          {
        //              if (logStartStop)
        //                  Logger.LogDebug(FUNCTION_START, func.Method.Name);

        //              result = func.Invoke();
        //          }
        //          catch (Exception ex)
        //          {
        //              Logger.LogError(ex, " - Errore nel metodo " + func.Method.Name);
        //          }
        //          finally
        //          {
        //              if (logStartStop )
        //                  Logger.LogDebug(FUNCTION_STOP, func.Method.Name);
        //          }

        //          return result;
        //      }
    }
}
