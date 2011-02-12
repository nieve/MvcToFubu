using System;
using System.Collections.Generic;

namespace MvcToFubu.Fubu
{
    public interface IBehaviorChainIdLookup
    {
        string GenerateKey(Type handlerType, string methodName, IEnumerable<KeyValuePair<string, Type>> parameters);
        Guid Lookup(string key);
    }
}