using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MvcToFubu.Mvc
{
    public class MvcToFubuControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public MvcToFubuControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            Controller result = null;
            if (controllerType != null)
            {
                result = (Controller)_container.GetInstance(controllerType);
                result.ActionInvoker = new MvcToFubuControllerActionInvoker(_container);
            }
            return result;
        }

        public bool IsValidControllerName(RequestContext requestContext, string controllerName)
        {
            return GetControllerType(requestContext, controllerName) != null;
        }
    }
}