using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_get_a_release
    {
        public class WHEN_the_release_does_not_exist
        {
            [Test]
            public void THEN_the_release_is_not_returned()
            {
                var isNotFound = false;
                var isFound = false;
                new GetReleaseService (() => isNotFound = true, () => isFound = true).Get();
                Assert.IsTrue (isNotFound);
                Assert.IsFalse (isFound);
            }
        }

        public class WHEN_the_release_exists
        {
            [Test]
            public void THEN_the_release_is_returned()
            {
                var isFound = false;
                var isNotFound = false;
                new GetReleaseService (() => isNotFound = true, () => isFound = true).Get();
                Assert.IsFalse (isNotFound);
                Assert.IsTrue (isFound);
            }
        }
    }

    public class GetReleaseService
    {
        readonly Action _onReleaseNotFound;
        readonly Action _onReleaseFound;

        public GetReleaseService (Action onReleaseNotFound, Action onReleaseFound)
        {
            _onReleaseNotFound = onReleaseNotFound;
            _onReleaseFound = onReleaseFound;
        }

        public void Get ()
        {
            _onReleaseNotFound ();   
        }
    }
}

