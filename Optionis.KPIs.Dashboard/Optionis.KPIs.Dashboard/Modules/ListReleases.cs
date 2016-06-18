using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Adapters;
using Optionis.KPIs.Common;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class ListReleases : NancyModule
    {
        public ListReleases ()
        {
            Get [Routing.Releases.ROUTE] = _ => {
                dynamic response = null;
                new ReleaseListingService (
                    new ReleaseLister (),
                    releases => response = new { releases }).List (NumberOfResultsToReturn ());
                return response;
            };
        }

        int NumberOfResultsToReturn ()
        {
            if (Request.Query ["top"].HasValue)
                return int.Parse (Request.Query ["top"].Value);
            return 5;
        }
    }
}
