using Optionis.KPIs.Dashboard.Core;
using Optionis.KPIs.Adapters;
using Nancy;

namespace Optionis.KPIs.Dashboard
{
    public class ListReleasesController : NancyModule
    {
        public ListReleasesController ()
        {
            Get["/releases"] = _ => new
            {
                Releases = new ReleasesLister(new ReleasesRepository()).List()
            };
        }
    }
}
