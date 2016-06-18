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
                var svc = new DeploymentDeletionService (() => releaseDeleted = true);
                svc.Delete (1);
                Assert.IsTrue (releaseDeleted);
            }
        }

        public class DeploymentDeletionService
        {
            readonly Action _onDeploymentDeleted;

            public DeploymentDeletionService (Action onDeploymentDeleted)
            {
                _onDeploymentDeleted = onDeploymentDeleted;
                
            }

            public void Delete (int deploymentId)
            {
                _onDeploymentDeleted ();
            }
        }
    }
}

