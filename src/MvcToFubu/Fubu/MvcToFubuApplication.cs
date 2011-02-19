using System.Web.Mvc;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using MvcToFubu.Mvc;
using StructureMap;
using StructureMap.Pipeline;

namespace MvcToFubu.Fubu
{
    public static class MvcToFubuApplication
    {
        public static void Start<T>(IContainer container) where T : FubuRegistry, new()
        {
            Start(new T(), container);
        }

        public static void Start(FubuRegistry registry, IContainer container)
        {
            var graph = registry.BuildGraph();
            var behaviorChainIdLookup = new BehaviorChainIdLookup(graph.Behaviors);
            container.Configure(x =>
            {
                x.For<IMvcAction>().HttpContextScoped().Use<MvcAction>();
                x.FillAllPropertiesOfType<IMvcAction>();
                x.For<IBehaviorChainIdLookup>().Singleton().Use(behaviorChainIdLookup);
            });
            graph.EachService((type, def) =>
            {
                if (def.Value == null)
                {
                    container.Configure(x => x.For(type).Add(new ObjectDefInstance(def)));
                }
                else
                {
                    container.Configure(x => x.For(type).Add(new ObjectInstance(def.Value)
                    {
                        Name = def.Name
                    }));
                }
            });

            ControllerBuilder.Current.SetControllerFactory(new MvcToFubuControllerFactory(container));
        }
    }
}