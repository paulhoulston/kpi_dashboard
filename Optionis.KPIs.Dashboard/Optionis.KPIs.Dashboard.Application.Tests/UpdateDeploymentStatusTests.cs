﻿using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_update_the_status_of_a_deployment
    {
        class TestRunner : DeploymentStatusUpdaterService.ICheckIfDeploymentsExist, DeploymentStatusUpdaterService.IUpdateDeploymentStatuses
        {
            public bool _statusNotUpdated { get; private set; }
            public bool _statusUpdated { get; private set; }
            readonly bool _deploymentExists;

            public TestRunner(bool deploymentExists)
            {
                _deploymentExists = deploymentExists;
                new DeploymentStatusUpdaterService(
                    () => _statusNotUpdated = true,
                    () => _statusUpdated = true,
                    this,
                    this).UpdateStatus(1, DeploymentStatus.Aborted);
            }

            public void IfDeploymentExists(int deploymentId, Action onDeploymentNotExists, Action onDeploymentExists)
            {
                if (_deploymentExists)
                    onDeploymentExists();
                else
                    onDeploymentNotExists();
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
                Assert.IsFalse(_testRunner._statusNotUpdated);
                Assert.IsTrue(_testRunner._statusUpdated);
            }
        }

        public class WHEN_the_deployment_cannot_be_found
        {
            readonly TestRunner _testRunner = new TestRunner(false);

            [Test]
            public void THEN_the_status_is_not_updated()
            {
                Assert.IsFalse(_testRunner._statusUpdated);
                Assert.IsTrue(_testRunner._statusNotUpdated);
            }
        }
    }
}
