using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_update_a_deployment_status
    {
        public class WHEN_I_specify_a_status_for_a_valid_deployment
        {
            [Test]
            public void THEN_the_status_is_updated()
            {
                var statusUpdated = false;
                Assert.IsTrue(statusUpdated);
            }
        }
    }
}
