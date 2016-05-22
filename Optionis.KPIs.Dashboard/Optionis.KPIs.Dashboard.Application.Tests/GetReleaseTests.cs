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
                new GetReleaseService (() => isNotFound = true).Get();
                Assert.IsTrue (isNotFound);
            }
        }

        /*public class WHEN_the_release_exists
        {
            [Test]
            public void THEN_the_release_is_returned()
            {
                var isFound = false;
                Assert.IsTrue (isFound);
            }
        }*/
    }

    public class GetReleaseService
    {
        readonly Action _onReleaseNotFound;

        public GetReleaseService (Action onReleaseNotFound)
        {
            _onReleaseNotFound = onReleaseNotFound;
        }

        public void Get ()
        {
            _onReleaseNotFound ();   
        }
    }
}

