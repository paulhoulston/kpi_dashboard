using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Modules.Routes;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class GetIssue : NancyModule
    {
        public GetIssue ()
        {
            Get [Routing.Issues.GET] = _ => {
                Response response = null;
                new GetIssueService (
                    new IssueRetriever (),
                    () => response = HttpStatusCode.NotFound,
                    issue => {
                        response = Newtonsoft.Json.JsonConvert.SerializeObject(Convert (issue));
                        response.StatusCode = HttpStatusCode.OK;
                        response.ContentType = "application/json";
                    }
                ).Get (_.IssueId);
                return response;
            };
        }

        static dynamic Convert(GetIssueService.Issue issue)
        {
            return new {
                links = new {
                    self = Routing.Issues.Get (issue.ReleaseId, issue.Id)
                },
                issue.IssueId,
                issue.Link,
                issue.Title
            };
        }
    }
}

