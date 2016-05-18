using Optionis.KPIs.Dashboard.Core;
using Optionis.KPIs.Adapters;
using Nancy;

namespace Optionis.KPIs.Dashboard
{
    public class ListReleasesController : NancyModule
    {
        public dynamic Get()
        {
            return new
            {
                Releases = new ReleasesLister(new ReleasesRepository()).List()
            };
        }
    }
    
}
