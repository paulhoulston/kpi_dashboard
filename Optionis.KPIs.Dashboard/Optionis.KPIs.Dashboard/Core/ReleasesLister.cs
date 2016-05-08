using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Adapters.DatabaseModels;

namespace Optionis.KPIs.Dashboard.Core
{
    class ReleasesLister
    {
        readonly IStoreReleases _repository;

        public interface IStoreReleases
        {
            IEnumerable<Release> GetAll();
        }

        public ReleasesLister (IStoreReleases repository)
        {
            _repository = repository;
        }

        public IEnumerable<Release> List ()
        {
            return _repository.GetAll ();
        }
    }
}
