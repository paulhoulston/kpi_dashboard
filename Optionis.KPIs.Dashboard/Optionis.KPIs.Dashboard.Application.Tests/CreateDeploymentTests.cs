using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_create_a_deployment
    {
        public class TestRunner : DeploymentCreationService.ICreateDeployments
        {
            public bool DeploymentCreated { get; private set; }
            public DeploymentCreationService.ValidationError ValidationError { get; set; }

            public TestRunner (DeploymentCreationService.Deployment deployment)
            {
                new DeploymentCreationService(
                    validationError => ValidationError = validationError,
                    _ => { },
                    this).CreateDeployment(deployment);        
            }

            public void CreateDeployment (DeploymentCreationService.Deployment deployment, Action<int> onDeploymentCreated)
            {
                DeploymentCreated = true;
                onDeploymentCreated (1);
            }
        }

        public class WHEN_the_deployment_is_valid
        {
            readonly TestRunner _testRunner = new TestRunner(new DeploymentCreationService.Deployment{
                Version = "1.2.3.4",
                DeploymentDate = DateTime.Now
            });

            [Test]
            public void THEN_the_deployment_is_created()
            {
                Assert.IsTrue (_testRunner.DeploymentCreated);
            }

            [Test]
            public void AND_no_error_code_is_returned()
            {
                Assert.AreEqual (DeploymentCreationService.ValidationError.None, _testRunner.ValidationError);
            }
        }

        [TestFixture(null)]
        [TestFixture("")]
        [TestFixture("1.")]
        [TestFixture("1.2")]
        [TestFixture("1.2.3")]
        [TestFixture("1.2.3.a")]
        [TestFixture("RGERsdfG")]
        public class WHEN_the_deployment_has_an_invalid_version_number
        {
            readonly TestRunner _testRunner;

            public WHEN_the_deployment_has_an_invalid_version_number (string version)
            {
                _testRunner = new TestRunner(new DeploymentCreationService.Deployment{
                    DeploymentDate = DateTime.Now,
                    Version = version
                });
            }

            [Test]
            public void THEN_the_deployment_is_not_created()
            {
                Assert.IsFalse (_testRunner.DeploymentCreated);
            }

            [Test]
            public void AND_the_version_number_is_invalid_error_code_is_returned()
            {
                Assert.AreEqual (DeploymentCreationService.ValidationError.InvalidVersionNumber, _testRunner.ValidationError);
            }
        }

        public class WHEN_the_deployment_date_is_more_than_90_days_old
        {
            readonly TestRunner _testRunner = new TestRunner (new DeploymentCreationService.Deployment {
                Version = "1.2.3.*",
                DeploymentDate = DateTime.Today.AddDays (-90).AddMilliseconds (-1)
            });

            [Test]
            public void THEN_the_deployment_is_not_created()
            {
                Assert.IsFalse (_testRunner.DeploymentCreated);
            }

            [Test]
            public void AND_the_deployent_date_too_old_error_code_is_returned()
            {
                Assert.AreEqual (DeploymentCreationService.ValidationError.InvalidDeploymentDate, _testRunner.ValidationError);
            }
        }
    }
}

