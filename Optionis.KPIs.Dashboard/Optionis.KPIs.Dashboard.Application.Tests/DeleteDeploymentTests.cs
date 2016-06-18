using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_delete_a_deployment
    {
        class TestRunner :
            DeploymentDeletionService.IDeleteDeployments,
            DeploymentDeletionService.IDeleteReleases,
            DeploymentDeletionService.ICheckIfReleaseHasDeployments,
            DeploymentDeletionService.ICheckIfDeploymentsExist
        {
            public bool DeploymentDeleted { get; private set; }
            public bool ReleaseDeleted { get; private set; }

            readonly List<int> _deployments;
            readonly int _releaseId;

            public TestRunner (int deploymentId, int releaseId, params int[] deployments)
            {
                _releaseId = releaseId;
                _deployments = new List<int>(deployments);
                new DeploymentDeletionService(this, this, this, this).DeleteDeployment(deploymentId);
            }

            public void DeleteDeployment(int deploymentId, Action onDeploymentDeleted)
            {
                if (_deployments.Contains (deploymentId)) {
                    _deployments.Remove (deploymentId);
                    onDeploymentDeleted ();
                }
                DeploymentDeleted = true;
            }

            public void DeleteRelease (int releaseId)
            {
                ReleaseDeleted = true;
            }

            public void DeploymentsAssigned (int releaseId, Action onHasDeployments)
            {
                if (!_deployments.Any ())
                    onHasDeployments ();
            }

            public void DeploymentExists (int deploymentId, Action<DeploymentDeletionService.Deployment> onDeploymentFound)
            {
                if (_deployments.Contains (deploymentId))
                    onDeploymentFound (new DeploymentDeletionService.Deployment {
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
                Assert.IsFalse (_testRunner.DeploymentDeleted);
            }

            [Test]
            public void AND_the_release_is_not_deleted()
            {
                Assert.IsFalse (_testRunner.ReleaseDeleted);
            }
        }

        public class WHEN_the_deployment_does_exist_AND_there_are_multiple_deployments_associated_with_the_release
        {
            readonly TestRunner _testRunner = new TestRunner (1, 2, 1, 2);

            [Test]
            public void THEN_the_deployment_is_deleted()
            {
                Assert.IsTrue (_testRunner.DeploymentDeleted);
            }

            [Test]
            public void AND_the_release_is_not_deleted()
            {
                Assert.IsFalse (_testRunner.ReleaseDeleted);
            }
        }

        public class WHEN_the_deployment_exists_AND_there_are_no_more_deployments_assigned_to_the_release
        {
            
            readonly TestRunner _testRunner = new TestRunner (1, 2, 1);

            [Test]
            public void THEN_the_deployment_is_deleted()
            {
                Assert.IsTrue (_testRunner.DeploymentDeleted);
            }

            [Test]
            public void AND_the_owning_release_is_not_deleted()
            {
                Assert.IsTrue (_testRunner.ReleaseDeleted);
            }
        }

        public class DeploymentDeletionService
        {
            readonly IDeleteDeployments _deploymentRepository;
            readonly IDeleteReleases _releaseRepository;
            readonly ICheckIfReleaseHasDeployments _releaseAssignedDeploymentChecker;
            readonly ICheckIfDeploymentsExist _deploymentExistenceChecker;

            public class Deployment
            {
                public int DeploymentId{ get; set; }
                public int ReleaseId { get; set; }
            }

            public interface ICheckIfDeploymentsExist
            {
                void DeploymentExists(int deploymentId, Action<Deployment> onDeploymentFound);
            }

            public interface IDeleteDeployments
            {
                void DeleteDeployment(int deploymentId, Action onDeploymentDeleted);
            }

            public interface IDeleteReleases
            {
                void DeleteRelease(int releaseId);
            }

            public interface ICheckIfReleaseHasDeployments
            {
                void DeploymentsAssigned(int releaseId, Action onNoDeploymentsAssigned);
            }

            public DeploymentDeletionService (
                IDeleteDeployments deploymentRepository,
                IDeleteReleases releaseRepository,
                ICheckIfReleaseHasDeployments releaseAssignedDeploymentChecker,
                ICheckIfDeploymentsExist deploymentExistenceChecker)
            {
                _deploymentExistenceChecker = deploymentExistenceChecker;
                _releaseAssignedDeploymentChecker = releaseAssignedDeploymentChecker;
                _releaseRepository = releaseRepository;
                _deploymentRepository = deploymentRepository;
            }

            public void DeleteDeployment (int deploymentId)
            {
                _deploymentExistenceChecker.DeploymentExists (deploymentId, OnDeploymentExists);
            }

            void OnDeploymentExists (Deployment deployment)
            {
                _deploymentRepository.DeleteDeployment (deployment.DeploymentId, 
                    () => OnDeploymentDeleted(deployment));
            }

            void OnDeploymentDeleted (Deployment deployment)
            {
                _releaseAssignedDeploymentChecker.DeploymentsAssigned (
                    deployment.ReleaseId,
                    () => _releaseRepository.DeleteRelease (deployment.ReleaseId));
            }
        }
    }
}

