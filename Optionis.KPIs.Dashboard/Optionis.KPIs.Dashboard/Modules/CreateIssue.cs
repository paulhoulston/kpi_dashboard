using Nancy;
using Optionis.KPIs.Dashboard.Modules.Routes;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class CreateIssue : NancyModule
    {
        public CreateIssue()
        {
            Post[Routing.Issues.ROUTE] = _ =>
            {
                return new SerializedError(null, "Not impemented", Response).ErrorResponse();
            };
        }
    }
}