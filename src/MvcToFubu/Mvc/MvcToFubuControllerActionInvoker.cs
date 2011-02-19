using System;
using System.Linq;
using System.Web.Mvc;
using FubuMVC.Core.Behaviors;
using MvcToFubu.Fubu;
using StructureMap;

namespace MvcToFubu.Mvc
{
    public class MvcToFubuControllerActionInvoker : ControllerActionInvoker
    {
        private readonly IContainer _container;

        public MvcToFubuControllerActionInvoker(IContainer container)
        {
            _container = container;
        }

        public override bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(actionName))
            {
                throw new ArgumentException("actionName");
            }

            var controllerDescriptor = GetControllerDescriptor(controllerContext);
            var actionDescriptor = FindAction(controllerContext, controllerDescriptor, actionName);
            if (actionDescriptor == null)
                return false;

            var filterInfo = GetFilters(controllerContext, actionDescriptor);
            var mvcAction = _container.GetInstance<IMvcAction>();
            mvcAction.ActionCall = () =>
                {
                    var authContext = InvokeAuthorizationFilters(controllerContext, filterInfo.AuthorizationFilters,
                                                                 actionDescriptor);
                    if (authContext.Result != null)
                    {
                        // the auth filter signaled that we should let it short-circuit the request
                        InvokeActionResult(controllerContext, authContext.Result);
                    }
                    else
                    {
                        if (controllerContext.Controller.ValidateRequest && !controllerContext.IsChildAction)
                        {
                            controllerContext.HttpContext.Request.ValidateInput();
                        }

                        var parameters = GetParameterValues(controllerContext, actionDescriptor);
                        var postActionContext = InvokeActionMethodWithFilters(controllerContext,
                                                                              filterInfo.ActionFilters,
                                                                              actionDescriptor,
                                                                              parameters);
                        InvokeActionResultWithFilters(controllerContext, filterInfo.ResultFilters,
                                                      postActionContext.Result);
                    }
                };

            //In the event that an ActionNameAttribute has been used to give a controller method a different ActionName
            //this will find the actual method name to help us find the correct behavior chain with
            var reflectedActionDescriptor = actionDescriptor as ReflectedActionDescriptor;
            if (reflectedActionDescriptor != null)
            {
                var methodName = reflectedActionDescriptor.MethodInfo.Name;
                if (!string.Equals(actionName, methodName, StringComparison.OrdinalIgnoreCase))
                {
                    actionName = methodName;
                }
            }

            var controllerType = controllerContext.Controller.GetType();
            var parameterDescriptors = actionDescriptor.GetParameters();
            var inputParameters = parameterDescriptors.ToDictionary(x => x.ParameterName, x => x.ParameterType);
            var lookup = _container.GetInstance<IBehaviorChainIdLookup>();
            var key = lookup.GenerateKey(controllerType, actionName, inputParameters);
            var guid = lookup.Lookup(key);
            _container.Configure(x =>
            {
                x.FillAllPropertiesOfType<ControllerContext>().Use(controllerContext);
                x.For<PartialBehavior>().Use(PartialBehavior.Ignored);
            });
            var actionBehavior = _container.GetInstance<IActionBehavior>(guid.ToString());
            actionBehavior.Invoke();
            return true;
        }
    }
}