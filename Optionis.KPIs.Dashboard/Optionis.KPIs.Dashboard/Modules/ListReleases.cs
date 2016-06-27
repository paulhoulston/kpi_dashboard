using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Common;
using System.Linq;
using Optionis.KPIs.DataAccess;

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
                    releases => response = new { releases = releases.Select (r => Routing.Releases.Get (r.ReleaseId))}).List (NumberOfResultsToReturn ());
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
