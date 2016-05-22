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
}

