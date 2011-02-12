using System;

namespace MvcToFubu.Mvc
{
    public interface IMvcAction
    {
        Action ActionCall { get; set; }
    }
}