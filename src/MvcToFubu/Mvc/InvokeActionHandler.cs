using FubuMVC.Core;
using FubuMVC.Core.Behaviors;

namespace MvcToFubu.Mvc
{
    public class InvokeActionHandler : BasicBehavior
    {
        public InvokeActionHandler()
            : base(PartialBehavior.Ignored)
        {
        }

        public IMvcAction Action { get; set; }

        protected override DoNext performInvoke()
        {
            if (Action != null)
            {
                Action.ActionCall();
            }
            return DoNext.Continue;
        }
    }
}