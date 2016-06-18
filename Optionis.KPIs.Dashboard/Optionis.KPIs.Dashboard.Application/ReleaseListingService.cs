using System.Collections.Generic;
using System;
using Optionis.KPIs.Common;

namespace Optionis.KPIs.Dashboard.Application
{
    public class ReleaseListingService
    {
        readonly IListReleases _releaseRepository;
        readonly Action<IEnumerable<Release>> _onReleasesRetrieved;

        public interface IListReleases
        {
            IEnumerable<ReleaseListingService.Release> List ();
        }

        public class Release
        {
            public Release (int releaseId)
            {
                Uri = Routing.Releases.Get(releaseId);
            }

            public string Uri{ get; private set; }
        }

        public ReleaseListingService (IListReleases releaseRepository, Action<IEnumerable<Release>> onReleasesRetrieved)
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

