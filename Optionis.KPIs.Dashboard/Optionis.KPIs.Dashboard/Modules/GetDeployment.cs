using Nancy;
using Optionis.KPIs.Common;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Adapters;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class GetDeployment : NancyModule
    {
        public GetDeployment ()
        {
            Get [Routing.Deployments.GET] = _ => {
                Response response = null;
                new GetDeploymentService (
                    new DeploymentRetriever (),
                    () => response = HttpStatusCode.NotFound,
                    issue => {
                        response = Newtonsoft.Json.JsonConvert.SerializeObject(Convert (issue));
                        response.StatusCode = HttpStatusCode.OK;
                        response.ContentType = "application/json";
                    }
                ).Get (_.Id);
                return response;
            };
        }

        static dynamic Convert(GetDeploymentService.Deployment deployment)
        {
            return new {
                Self = Routing.Deployments.Get (deployment.Id),
                deployment.DeploymentDate,
                deployment.Version,
                status = deployment.Status.ToString()
            };
        }
    }
}

