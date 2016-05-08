using System.Web.Http;
using Optionis.KPIs.Adapters;
using Optionis.KPIs.Dashboard.Core;
using System.Net.Http;
using System.Net;

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

        public HttpResponseMessage Post(CreateReleaseModel releaseToCreate)
        {
            return Request.CreateResponse (HttpStatusCode.Created);
        }

        public class CreateReleaseModel
        {
        }
    }
}

