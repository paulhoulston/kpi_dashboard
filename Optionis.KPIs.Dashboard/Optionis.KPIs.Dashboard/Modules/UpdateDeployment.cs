using Nancy;
using Optionis.KPIs.Common;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class UpdateDeployment : NancyModule
    {
        public UpdateDeployment()
        {
            Patch[Routing.Deployments.PATCH] = _ =>
            {
                return HttpStatusCode.NoContent;
            };
        }
    }
}