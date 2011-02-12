using System;

namespace MvcToFubu.Mvc
{
    public class MvcAction : IMvcAction
    {
        public MvcAction()
        {
            ActionCall = () => { };
        }

        public Action ActionCall { get; set;}
    }
}