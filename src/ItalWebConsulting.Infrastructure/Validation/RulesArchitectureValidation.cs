using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace InfoWeb.Infrastructure.Validation
{
    public static class RulesArchitectureValidation
    {
        public static void CheckIfDependency(Assembly assembly, params Assembly[] forbiddenAssemblies)
        {
            if (assembly == null)
                return;
            if (forbiddenAssemblies == null)
                return;
            var dpdAss = assembly.GetReferencedAssemblies();
            foreach (var a in forbiddenAssemblies)
            {
                if (dpdAss.Any(da => da.FullName == a.FullName))
                    throw new Exception(String.Format("Il tipo project {0} non può referenziare il project {1}. Rimuovere la reference", assembly.FullName, a.FullName));
            }
        }
    }
}
