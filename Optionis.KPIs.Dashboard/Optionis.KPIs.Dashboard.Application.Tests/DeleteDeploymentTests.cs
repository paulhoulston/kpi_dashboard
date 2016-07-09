using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Optionis.KPIs.Dashboard.Application.Interfaces;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_delete_a_deployment
    {
        class TestRunner :
            DeploymentDeletionService.IDeleteDeployments,
            DeploymentDeletionService.IDeleteReleases,
            DeploymentDeletionService.ICheckIfReleaseHasDeployments,
            ICheckIfDeploymentsExist
        {
            public bool DeploymentDeleted { get; private set; }
            public bool ReleaseDeleted { get; private set; }

            readonly List<int> _deployments;
            readonly int _releaseId;

            public TestRunner(int deploymentId, int releaseId, params int[] deployments)
            {
                _releaseId = releaseId;
                _deployments = new List<int>(deployments);
                new DeploymentDeletionService(this, this, this, this).DeleteDeployment(deploymentId);
            }

            public void DeleteDeployment(int deploymentId, Action onDeploymentDeleted)
            {
                if (_deployments.Contains(deploymentId))
                {
                    _deployments.Remove(deploymentId);
                    onDeploymentDeleted();
                }
                DeploymentDeleted = true;
            }

            public void DeleteRelease(int releaseId)
            {
                ReleaseDeleted = true;
            }

            public void DeploymentsAssigned(int releaseId, Action onHasDeployments)
            {
                if (!_deployments.Any())
                    onHasDeployments();
            }

            public void DeploymentExists(int deploymentId, Action<Deployment> onDeploymentFound)
            {
                if (_deployments.Contains(deploymentId))
                    onDeploymentFound(new Deployment
                    {
                        ReleaseId = _releaseId,
                        DeploymentId = deploymentId
                    });
            }
        }

        public class WHEN_the_deployment_does_not_exist
        {
            readonly TestRunner _testRunner = new TestRunner(-1, -1, 0);

            [Test]
            public void THEN_deployment_is_not_deleted()
            {
                Assert.IsFalse(_testRunner.DeploymentDeleted);
            }

            [Test]
            public void AND_the_release_is_not_deleted()
            {
                Assert.IsFalse(_testRunner.ReleaseDeleted);
            }
        }

        public class WHEN_the_deployment_does_exist_AND_there_are_multiple_deployments_associated_with_the_release
        {
            readonly TestRunner _testRunner = new TestRunner(1, 2, 1, 2);

            [Test]
            public void THEN_the_deployment_is_deleted()
            {
                Assert.IsTrue(_testRunner.DeploymentDeleted);
            }

            [Test]
            public void AND_the_release_is_not_deleted()
            {
                Assert.IsFalse(_testRunner.ReleaseDeleted);
            }
        }

        public class WHEN_the_deployment_exists_AND_there_are_no_more_deployments_assigned_to_the_release
        {
            readonly TestRunner _testRunner = new TestRunner(1, 2, 1);

            [Test]
            public void THEN_the_deployment_is_deleted()
            {
                Assert.IsTrue(_testRunner.DeploymentDeleted);
            }

            [Test]
            public void AND_the_owning_release_is_not_deleted()
            {
                Assert.IsTrue(_testRunner.ReleaseDeleted);
            }
        }
    }
}
