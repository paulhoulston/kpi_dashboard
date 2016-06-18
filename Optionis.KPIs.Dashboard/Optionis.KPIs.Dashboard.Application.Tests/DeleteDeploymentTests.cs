using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_delete_a_deployment
    {
        class TestRunner : DeploymentDeletionService.IDeleteDeployments
        {
            public bool DeploymentDeleted { get; private set; }
            public bool ReleaseDeleted { get; private set; }

            public TestRunner (int deploymentId)
            {
                new DeploymentDeletionService(() => DeploymentDeleted = true, this).Delete(deploymentId);
            }

            public void Delete(int deploymentId, Action onDeploymentDeleted)
            {
                DeploymentDeleted = true;
            }
        }

        public class WHEN_the_deployment_does_not_exist
        {
            readonly TestRunner _testRunner = new TestRunner(-1);

            [Test]
            public void THEN_deployment_deleted_delegate_is_executed()
            {
                Assert.IsTrue (_testRunner.DeploymentDeleted);
            }


            [Test]
            public void AND_the_release_is_not_deleted()
            {
                Assert.IsFalse (_testRunner.ReleaseDeleted);
            }
        }

        public class WHEN_the_deployment_does_exist_AND_there_are_multiple_deployments_associated_with_the_release
        {
            TestRunner _testRunner = new TestRunner (1);

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
            
            readonly TestRunner _testRunner = new TestRunner (1);

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
            readonly Action _onDeploymentDeleted;
            readonly IDeleteDeployments _repository;

            public interface IDeleteDeployments
            {
                void Delete(int deploymentId, Action onDeploymentDeleted);
            }

            public DeploymentDeletionService (Action onDeploymentDeleted, IDeleteDeployments repository)
            {
                _repository = repository;
                _onDeploymentDeleted = onDeploymentDeleted;
                
            }

            public void Delete (int deploymentId)
            {
                _repository.Delete (deploymentId, _onDeploymentDeleted);
            }
        }
    }
}

