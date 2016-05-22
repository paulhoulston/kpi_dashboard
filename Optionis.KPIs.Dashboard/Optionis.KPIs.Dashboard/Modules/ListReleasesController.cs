using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Adapters;
using Optionis.KPIs.Common;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class ListReleasesController : NancyModule
    {
        public ListReleasesController ()
        {
            Get [Routing.Releases.ROUTE] = _ => {
                dynamic response = null;
                new RelaseListingService (
                    new ReleaseLister (),
                    releases => response = new { releases }).List ();
                return response;
            };
        }
    }
}
