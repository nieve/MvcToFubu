using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core.Registration.Nodes;

namespace MvcToFubu.Fubu
{
    public class BehaviorChainIdLookup : IBehaviorChainIdLookup
    {
        private const string KeyFormat = "{0}.{1}({2})";
        private readonly ConcurrentDictionary<string, Guid> _lookup;

        public BehaviorChainIdLookup(IEnumerable<BehaviorChain> behaviorChains)
        {
            _lookup = new ConcurrentDictionary<string, Guid>(
                behaviorChains.ToDictionary(x =>
                {
                    var actionCall = x.FirstCall();
                    var parameters = actionCall.Method.GetParameters()
                        .ToDictionary(y => y.Name, y => y.ParameterType);
                    return GenerateKey(actionCall.HandlerType, actionCall.Method.Name, parameters);
                }, x => x.UniqueId)
                );
        }

        public string FormatParameters(IEnumerable<KeyValuePair<string, Type>> parameters)
        {
            return parameters.Select(x => "{0} {1}".ToFormat(x.Value.Name, x.Key)).Join(", ");
        }

        public string GenerateKey(Type handlerType, string methodName, IEnumerable<KeyValuePair<string, Type>> parameters)
        {
            return KeyFormat.ToFormat(handlerType, methodName, FormatParameters(parameters)).ToLowerInvariant();
        }

        public Guid Lookup(string key)
        {
            return _lookup[key];
        }
    }
}