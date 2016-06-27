using System.Collections.Generic;
using System;

namespace Optionis.KPIs.Dashboard.Application
{
    public class ReleaseListingService
    {
        readonly IListReleases _releaseRepository;
        readonly Action<IEnumerable<Release>> _onReleasesRetrieved;

        public interface IListReleases
        {
            IEnumerable<Release> List (int top);
        }

        public class Release
        {
            public int ReleaseId { get; set; }
        }

        public ReleaseListingService (IListReleases releaseRepository, Action<IEnumerable<Release>> onReleasesRetrieved)
        {
            _releaseRepository = releaseRepository;
            _onReleasesRetrieved = onReleasesRetrieved;
        }

        public void List(int top)
        {
            _onReleasesRetrieved (_releaseRepository.List (top));
        }
    }
}

