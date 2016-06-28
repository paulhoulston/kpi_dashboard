using Nancy;
using Optionis.KPIs.Common;
using Optionis.KPIs.Dashboard.Application;
using System.Linq;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class GetRelease : NancyModule
    {
        public GetRelease ()
        {
            //Get[Routing.Releases.GET, runAsync: true] = async (parameters, token) => await PerformGet (parameters, token);
            Get [Routing.Releases.GET] = _ => {
                Response response = null;
                new GetReleaseService (
                    new ReleaseRetriever (),
                    () => response = HttpStatusCode.NotFound,
                    release => {
                        response = Newtonsoft.Json.JsonConvert.SerializeObject(Convert (release));
                        response.StatusCode = HttpStatusCode.OK;
                        response.ContentType = "application/json";
                    }
                ).Get (_.Id);
                return response;
            };
        }

        static dynamic Convert(GetReleaseService.Release release)
        {
            return new {
                links = new {
                    self = Routing.Releases.Get (release.Id),
                    issues = new {
                        list = release.IssueIds.NullSafe().Select (issueId => Routing.Issues.Get (release.Id, issueId))
                    },
                    deployments = new {
                        add = Routing.Deployments.Add(release.Id),
                        list = release.DeploymentIds.NullSafe().Select (deploymentId => Routing.Deployments.Get (release.Id, deploymentId))
                    },
                },
                release.Title,
                release.Created,
                release.CreatedBy,
                release.Application,
                release.Comments
            };
        }

        /*async dynamic PerformGet(dynamic parameters, CancellationToken _)
        {
        }*/
    }
}

