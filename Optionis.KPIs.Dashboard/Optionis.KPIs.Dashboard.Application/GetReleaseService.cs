using System;
using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard.Application
{
    public class GetReleaseService
    {
        readonly Action _onReleaseNotFound;
        readonly Action<Release> _onReleaseFound;
        readonly IGetReleases _repository;

        public interface IGetReleases
        {
            void Get(int releaseId, Action onReleaseNotFound, Action<Release> onReleaseFound);
        }

        public class Release
        {
            public int Id { get; set; }
            public string Title{ get; set; }
            public string Comments { get; set; }
            public DateTime Created{ get; set; }
            public string CreatedBy{ get; set; }
            public string Application{ get; set; }
            public IEnumerable<int> IssueIds{ get; set; }
            public IEnumerable<int> DeploymentIds{ get; set; }
        }

        public GetReleaseService (IGetReleases repository, Action onReleaseNotFound, Action<Release> onReleaseFound)
        {
            _repository = repository;
            _onReleaseNotFound = onReleaseNotFound;
            _onReleaseFound = onReleaseFound;
        }

        public void Get (int releaseId)
        {
            _repository.Get (releaseId, _onReleaseNotFound, _onReleaseFound);
        }
    }
}

