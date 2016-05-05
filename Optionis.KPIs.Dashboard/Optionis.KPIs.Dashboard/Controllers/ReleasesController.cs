using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Optionis.KPIs.Dashboard
{
    public class ReleasesController : ApiController
    {
        readonly IStoreReleases _repository;

        public ReleasesController ()
        {
            _repository = new ReleasesRepository ();
        }

        public dynamic Get()
        {
            return new
            {
                Releases = _repository.GetAll ()
            };
        }
    }

    interface IStoreReleases
    {
        IEnumerable<Release> GetAll();
    }

    class Release
    {
    }

    class ReleasesRepository : IStoreReleases
    {
        public IEnumerable<Release> GetAll ()
        {
            return new Release[]{ };
        }
    }
}

