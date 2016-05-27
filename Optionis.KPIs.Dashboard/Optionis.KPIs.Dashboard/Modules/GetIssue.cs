using Nancy;
using Optionis.KPIs.Common;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Adapters;

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
                ).Get (_.Id);
                return response;
            };
        }

        static dynamic Convert(GetIssueService.Issue issue)
        {
            return new {
                Self = Routing.Issues.Get (issue.Id),
                issue.IssueId,
                issue.Link,
                issue.Title
            };
        }
    }
}

