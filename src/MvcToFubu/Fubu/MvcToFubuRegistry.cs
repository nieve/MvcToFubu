using System.Web.Mvc;
using FubuMVC.Core;
using MvcToFubu.Mvc;

namespace MvcToFubu.Fubu
{
    public class MvcToFubuRegistry : FubuRegistry
    {
        public MvcToFubuRegistry()
        {
            var assembly = GetType().Assembly;
            Applies.ToAssembly(assembly);

            Actions
                .IgnoreMethodsDeclaredBy<Controller>()
                .IncludeTypesNamed(x => x.EndsWith("Controller"));

            ActionCallProvider((type, method) => new MvcActionCall(type, method));
        }
    }
}