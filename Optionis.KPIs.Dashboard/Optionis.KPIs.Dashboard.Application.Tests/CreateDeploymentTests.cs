using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_create_a_deployment
    {
        public class WHEN_the_deployment_is_valid
        {
            [Test]
            public void THEN_the_deployment_is_created()
            {
                var deploymentCreated = false;
                Assert.IsTrue (deploymentCreated);
            }

            [Test, Ignore]
            public void AND_no_error_code_is_returned()
            {
            }
        }

        public class WHEN_the_deployment_has_no_version_number
        {
            [Test, Ignore]
            public void THEN_the_deployment_is_not_created()
            {
            }

            [Test, Ignore]
            public void AND_the_version_number_is_invalid_error_code_is_returned()
            {
            }
        }

        public class WHEN_the_deployment_has_no_status
        {
            [Test, Ignore]
            public void THEN_the_deployment_is_not_created()
            {
            }

            [Test, Ignore]
            public void AND_the_status_is_not_set_error_code_is_returned()
            {
            }
        }

        public class WHEN_the_deployment_date_is_more_than_30_days_old
        {
            [Test, Ignore]
            public void THEN_the_deployment_is_not_created()
            {
            }

            [Test, Ignore]
            public void AND_the_deployent_date_too_old_error_code_is_returned()
            {
            }
        }
    }
}

