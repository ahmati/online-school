using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Composition
{
    public static class ContainerExtension
    {
        public static IEnumerable<string> GetRegistrationCatalog(this IContainer container)
        {
            var ls = new List<string>();
            foreach (var componentRegistration in container.ComponentRegistry.Registrations)
            {
                foreach (var registrationService in componentRegistration.Services)
                {
                    var registeredTargetType = registrationService.Description;
                    ls.Add(registeredTargetType);
                }
            }

            return ls;
        }
    }
}
