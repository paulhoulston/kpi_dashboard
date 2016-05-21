using System.Collections.Generic;
using System;

namespace Optionis.KPIs.Dashboard.Application
{
    public class RelaseListingService
    {
        readonly IListReleases _releaseRepository;
        readonly Action<IEnumerable<Release>> _onReleasesRetrieved;

        public interface IListReleases
        {
            IEnumerable<RelaseListingService.Release> List ();
        }

        public class Release
        {
            const string RELEASE_URI = "/releases/{id}";

            public Release (int releaseId)
            {
                Uri= RELEASE_URI.Replace (@"{id}", releaseId.ToString ());
            }

            public string Uri{ get; private set; }
        }

        public RelaseListingService (IListReleases releaseRepository, Action<IEnumerable<Release>> onReleasesRetrieved)
        {
            _releaseRepository = releaseRepository;
            _onReleasesRetrieved = onReleasesRetrieved;
        }

        public void List()
        {
            _onReleasesRetrieved (_releaseRepository.List ());
        }
    }
}

