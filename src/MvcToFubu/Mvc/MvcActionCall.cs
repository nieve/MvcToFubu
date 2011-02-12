using System;
using System.Reflection;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;

namespace MvcToFubu.Mvc
{
    public class MvcActionCall : ActionCall
    {
        public MvcActionCall(Type handlerType, MethodInfo method) : base(handlerType, method)
        {
        }

        public override ObjectDef ToObjectDef()
        {
            return new ObjectDef
            {
                Type = typeof (InvokeActionHandler)
            };
        }
    }
}