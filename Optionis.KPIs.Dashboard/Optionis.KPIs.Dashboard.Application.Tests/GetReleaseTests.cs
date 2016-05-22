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

                Assert.IsTrue (isNotFound);
            }
        }
    }
}

