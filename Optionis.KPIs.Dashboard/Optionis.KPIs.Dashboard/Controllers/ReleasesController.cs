using System.Web.Http;
using Optionis.KPIs.Adapters;
using Optionis.KPIs.Dashboard.Core;

namespace Optionis.KPIs.Dashboard
{
    public class ReleasesController : ApiController
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

