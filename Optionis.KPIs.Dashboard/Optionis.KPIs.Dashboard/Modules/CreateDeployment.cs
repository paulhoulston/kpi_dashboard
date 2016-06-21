using Nancy;
using Optionis.KPIs.Common;

namespace Optionis.KPIs.Dashboard
{
    public class CreateDeployment : NancyModule
    {
        public CreateDeployment ()
        {
            Post [Routing.Deployments.ROUTE] = _ => {
                return HttpStatusCode.Created;
            };
        }
    }
}

