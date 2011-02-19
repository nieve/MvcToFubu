using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using MvcToFubu.Fubu;
using MvcToFubu.Mvc;
using StructureMap;

namespace Sample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Example", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            MvcToFubuApplication.Start<SampleRegistry>(ObjectFactory.Container);
        }
    }

    public class SampleRegistry : MvcToFubuRegistry
    {
        public SampleRegistry()
        {
            Policies.Add<ExamplePolicy>();
        }
    }

    public class ExamplePolicy : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
                .Where(x => x.HandlerType.Name.StartsWith("Example"))
                .Each(x => x.AddBefore(new Wrapper(typeof(ExampleBehavior))));
        }
    }

    public class ExampleBehavior : BasicBehavior
    {
        public ExampleBehavior() : base(PartialBehavior.Ignored)
        {
        }

        protected override DoNext performInvoke()
        {
            //TODO: Do what you want here...
            Debug.WriteLine("Executing example behavior...");
            return DoNext.Continue;
        }
    }
}