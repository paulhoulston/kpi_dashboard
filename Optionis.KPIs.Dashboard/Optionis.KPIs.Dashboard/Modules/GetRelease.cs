using Nancy;
using Optionis.KPIs.Common;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Adapters;
using System.Linq;

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
                Self = Routing.Releases.Get (release.Id),
                release.Title,
                release.Created,
                release.CreatedBy,
                release.Application,
                release.Comments,
                Issues = release.IssueIds.Select (i => new { issue = Routing.Issues.Get (i)}),
                Deployments = release.DeploymentIds.Select (d => new { deployment = Routing.Deployments.Get (d)}),
            };
        }

        /*async dynamic PerformGet(dynamic parameters, CancellationToken _)
        {
        }*/
    }
}

