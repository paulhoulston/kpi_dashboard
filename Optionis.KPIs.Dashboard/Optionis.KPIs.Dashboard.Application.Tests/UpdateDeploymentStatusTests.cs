using System;
using NUnit.Framework;
using Optionis.KPIs.Dashboard.Application.Interfaces;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_update_the_status_of_a_deployment
    {
        class TestRunner : ICheckIfDeploymentsExist, DeploymentStatusUpdaterService.IUpdateDeploymentStatuses
        {
            public bool _statusUpdated { get; private set; }
            readonly bool _deploymentExists;

            public TestRunner(bool deploymentExists)
            {
                _deploymentExists = deploymentExists;
                new DeploymentStatusUpdaterService(
                    () => _statusUpdated = true,
                    this,
                    this).UpdateStatus(1, DeploymentStatus.Aborted);
            }

            public void DeploymentExists(int deploymentId, Action<Deployment> onDeploymentExists)
            {
                if (_deploymentExists)
                    onDeploymentExists(null);
            }

            public void Update(int deploymentId, DeploymentStatus status, Action onDeploymentUpdated)
            {
                onDeploymentUpdated();
            }
        }

        public class WHEN_I_specify_a_status_for_a_valid_deployment
        {
            readonly TestRunner _testRunner = new TestRunner(true);

            [Test]
            public void THEN_the_status_is_updated()
            {
                Assert.IsTrue(_testRunner._statusUpdated);
            }
        }
    }
}
