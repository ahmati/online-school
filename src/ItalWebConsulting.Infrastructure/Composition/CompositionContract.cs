using ItalWebConsulting.Infrastructure.Common;
using System;
using System.Collections.Generic;

namespace ItalWebConsulting.Infrastructure.Composition
{
    public class CompositionRegisterInput
    {
        //public string BasePath { get; set; }
        public Type AssemblyMvcType { get; set; }
        public IDictionary<string, AssemblyCompositionRegisterInput> ConnStringKeyAndContext { get; set; }
        public IList<FreeCompositionRegisterInput> FreeRegistration { get; set; }
        public IList<SingletonCompositionRegisterInput> SingletonRegistration { get; set; }
        public IList<AssemblyCompositionRegisterInput> AssemblyRepositoryType { get; set; }
        public IList<Type> AssemblyCoreType { get; set; }
        public IList<Type> AssemblyToRegister { get; set; }
    }
    public class FreeCompositionRegisterInput: SingletonCompositionRegisterInput
    {
        public RegistrationType RegistrationType { get; set; }
    }

    public class SingletonCompositionRegisterInput
    {
        public Type ObjectType { get; set; }
        public Type MappedInterface { get; set; }
        public StrCodeObjValue CtorParam { get; set; }
        public bool IsGeneric { get; set; }
    }

    public class AssemblyCompositionRegisterInput
    {
        public RegistrationType RegistrationType { get; set; }
        public Type ObjectType { get; set; }
    }
    public enum RegistrationType
    {
        Default = 0,
        ForRequest
    }
}
