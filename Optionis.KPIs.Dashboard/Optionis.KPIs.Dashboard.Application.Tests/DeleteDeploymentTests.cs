using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_delete_a_deployment
    {
        public class WHEN_the_deployment_does_not_exist
        {
            [Test]
            public void THEN_success_is_reported()
            {
                var releaseDeleted = false;

                Assert.IsTrue (releaseDeleted);
            }
        }
    }
}

