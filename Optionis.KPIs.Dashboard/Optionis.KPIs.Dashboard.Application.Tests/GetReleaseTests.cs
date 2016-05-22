using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_get_a_release
    {
        public class WHEN_the_release_does_not_exist : GetReleaseService.IGetReleases
        {
            public void Get (int releaseId, Action onReleaseNotFound, Action<GetReleaseService.Release> onReleaseFound)
            {
                onReleaseNotFound ();
            }

            [Test]
            public void THEN_the_release_is_not_returned()
            {
                var isNotFound = false;
                GetReleaseService.Release release = null;
                new GetReleaseService (this, () => isNotFound = true, _ => release = _).Get (0);
                Assert.IsTrue (isNotFound);
                Assert.IsNull (release);
            }
        }

        public class WHEN_the_release_exists : GetReleaseService.IGetReleases
        {
            public void Get (int releaseId, Action onReleaseNotFound, Action<GetReleaseService.Release> onReleaseFound)
            {
                onReleaseFound (new GetReleaseService.Release());
            }

            [Test]
            public void THEN_the_release_is_returned()
            {
                var isNotFound = false;
                GetReleaseService.Release release = null;
                new GetReleaseService (this, () => isNotFound = true, _ => release = _).Get(0);
                Assert.IsFalse (isNotFound);
                Assert.IsNotNull(release);
            }
        }
    }

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
            public int Id { private get; set; }
            public string Title{ get; set; }
            public string Created{ get; set; }
            public string CreatedBy{ get; set; }
            public string Application{ get; set; }
            public int[] IssueIds{ get; set; }
            public int[] DeploymentIds{ get; set; }
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

