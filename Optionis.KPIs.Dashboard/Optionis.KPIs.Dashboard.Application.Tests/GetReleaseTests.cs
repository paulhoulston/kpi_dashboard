using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_get_a_release
    {
        public class WHEN_the_release_does_not_exist : GetReleaseService.IGetReleases
        {
            public void Get (int releaseId, Action onReleaseNotFound, Action onReleaseFound)
            {
                onReleaseNotFound ();
            }

            [Test]
            public void THEN_the_release_is_not_returned()
            {
                var isNotFound = false;
                var isFound = false;
                new GetReleaseService (this, () => isNotFound = true, () => isFound = true).Get(0);
                Assert.IsTrue (isNotFound);
                Assert.IsFalse (isFound);
            }
        }

        public class WHEN_the_release_exists : GetReleaseService.IGetReleases
        {
            public void Get (int releaseId, Action onReleaseNotFound, Action onReleaseFound)
            {
                onReleaseFound ();
            }

            [Test]
            public void THEN_the_release_is_returned()
            {
                var isFound = false;
                var isNotFound = false;
                new GetReleaseService (this, () => isNotFound = true, () => isFound = true).Get(0);
                Assert.IsFalse (isNotFound);
                Assert.IsTrue (isFound);
            }
        }
    }

    public class GetReleaseService
    {
        readonly Action _onReleaseNotFound;
        readonly Action _onReleaseFound;
        readonly IGetReleases _repository;

        public interface IGetReleases
        {
            void Get(int releaseId, Action onReleaseNotFound, Action onReleaseFound);
        }

        public GetReleaseService (IGetReleases repository, Action onReleaseNotFound, Action onReleaseFound)
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

