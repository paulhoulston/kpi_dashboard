using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Adapters;

namespace Optionis.KPIs.Dashboard
{
    public class ListReleasesController : NancyModule
    {
        public ListReleasesController ()
        {
            Get ["/releases"] = _ => {
                dynamic response = null;
                new RelaseListingService (
                    new ReleaseLister (),
                    releases => response = new { releases }).List ();
                return response;
            };
        }
    }
}
