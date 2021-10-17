using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using ItalWebConsulting.Infrastructure.Composition;
using Autofac;
using ItalWebConsulting.Infrastructure.Logging;

namespace ItalWebConsulting.Infrastructure.ServiceApp
{
    public class ServiceAppUtils
    {
        Assembly[] servicesAssemblies;
        //IContainer container;

        public ServiceAppUtils(params Assembly[] servicesAssemblies)
        {
            //this.container = container;
            this.servicesAssemblies = servicesAssemblies;
        }
        public Type GetRunnableService(string[] batchArgs)
        {
            if (batchArgs == null || !batchArgs.Any())
                throw new ArgumentNullException(nameof(batchArgs), "Il primo elemento dell'imput args di un batch deve essere il serviceKey del servizio che si vuole avviare");
            var serviceKey = batchArgs[0];
            if (string.IsNullOrWhiteSpace(serviceKey))
                throw new ArgumentNullException(nameof(batchArgs), "Il primo elemento dell'imput args di un batch deve essere il serviceKey del servizio che si vuole avviare");
            LogConfigurator.ChangeFileLogPath(serviceKey);
            var rs = GetRunnableService(serviceKey);
            var isa = rs.GetInterface(typeof(IServiceApp).Name);
            if (isa == null)
                throw new Exception("Per avviare il servizio con serviceKey = " + serviceKey + " bisogna prima che questo implementi l'interfaccia " + typeof(IServiceApp).Name);

            //if (container.IsRegistered(rs))
            //{
            //    return (IServiceApp)container.Resolve(rs);
            //}

            //var interfaces= rs.GetInterfaces();
            //foreach (var i in interfaces)
            //{
            //    if (container.IsRegistered(i))
            //        return (IServiceApp)container.Resolve(i);
            //}

            //throw new Exception("Nessun componente registrato in AutoFac per il serviceKey in input = " + serviceKey);
            return rs;
        }

        private Type GetRunnableService(string serviceKey)
        {
            var services = GetAllRunnableService();
            foreach (var s in services)
            {
                var attrs = System.Attribute.GetCustomAttributes(s);
                var skTest = attrs.FirstOrDefault(f => f is ServiceKeyAttribute);
                if(skTest != null)
                {
                    var sk = (ServiceKeyAttribute)skTest;
                    if (sk.ServiceKey.Equals(serviceKey))
                        return s;
                }
            }

            throw new ArgumentException("Nessun servizio trovato con attributo ServiceKeyAttribute.ServiceKey = " + serviceKey);
        }

        private IList<Type> GetAllRunnableService()
        {
            return servicesAssemblies.SelectMany(x => x.GetTypes())
                 .Where(x => typeof(IServiceApp).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .ToList();
        }
    }
}
