using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MvcToFubu.Mvc
{
    public class MvcToFubuControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            Controller result = null;
            if (controllerType != null)
            {
                result = (Controller)ObjectFactory.Container.GetInstance(controllerType);
                result.ActionInvoker = new MvcToFubuControllerActionInvoker();
            }
            return result;
        }

        public bool IsValidControllerName(RequestContext requestContext, string controllerName)
        {
            return GetControllerType(requestContext, controllerName) != null;
        }
    }
}