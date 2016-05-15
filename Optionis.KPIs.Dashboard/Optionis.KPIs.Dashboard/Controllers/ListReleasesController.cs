using System.Web.Http;
using Optionis.KPIs.Dashboard.Attributes;
using Optionis.KPIs.Dashboard.Core;
using Optionis.KPIs.Adapters;

namespace Optionis.KPIs.Dashboard
{
    public class ListReleasesController : ApiController
    {
        [ListReleases]
        public dynamic Get()
        {
            return new
            {
                Releases = new ReleasesLister(new ReleasesRepository()).List()
            };
        }
    }
    
}
