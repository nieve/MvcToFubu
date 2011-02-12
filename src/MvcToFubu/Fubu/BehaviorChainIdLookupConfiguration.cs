using FubuMVC.Core.Registration;
using StructureMap;

namespace MvcToFubu.Fubu
{
    public class BehaviorChainIdLookupConfiguration : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var behaviorChainIdLookup = new BehaviorChainIdLookup(graph.Behaviors);
            ObjectFactory.Container.Configure(x => x.For<IBehaviorChainIdLookup>().Singleton().Use(behaviorChainIdLookup));
        }
    }
}