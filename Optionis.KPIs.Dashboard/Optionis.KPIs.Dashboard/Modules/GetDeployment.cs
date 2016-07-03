using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Modules.Routes;
using Optionis.KPIs.DataAccess;

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
                    deployment => {
                        response = Newtonsoft.Json.JsonConvert.SerializeObject(Convert (deployment));
                        response.StatusCode = HttpStatusCode.OK;
                        response.ContentType = "application/json";
                    }
                ).Get (_.DeploymentId);
                return response;
            };
        }

        static dynamic Convert(GetDeploymentService.Deployment deployment)
        {
            return new {
                links = new {
                    self = Routing.Deployments.Get (deployment.ReleaseId, deployment.DeploymentId)
                },
                deployment.DeploymentDate,
                deployment.Version,
                status = deployment.DeploymentStatus.ToString()
            };
        }
    }
}

