using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_get_a_deployment
    {
        public class WHEN_the_deployment_does_not_exist : GetDeploymentService.IGetDeployments
        {
            public void Get (int deploymentId, Action onDeploymentNotFound, Action<GetDeploymentService.Deployment> onDeploymentFound)
            {
                onDeploymentNotFound ();
            }

            [Test]
            public void THEN_the_Deployment_is_not_returned()
            {
                var isNotFound = false;
                GetDeploymentService.Deployment deployment = null;
                new GetDeploymentService (this, () => isNotFound = true, _ => deployment = _).Get (0);
                Assert.IsTrue (isNotFound);
                Assert.IsNull (deployment);
            }
        }

        public class WHEN_the_Deployment_exists : GetDeploymentService.IGetDeployments
        {
            public void Get (int deploymentId, Action onDeploymentNotFound, Action<GetDeploymentService.Deployment> onDeploymentFound)
            {
                onDeploymentFound (new GetDeploymentService.Deployment());
            }

            [Test]
            public void THEN_the_Deployment_is_returned()
            {
                var isNotFound = false;
                GetDeploymentService.Deployment deployment = null;
                new GetDeploymentService (this, () => isNotFound = true, _ => deployment = _).Get(0);
                Assert.IsFalse (isNotFound);
                Assert.IsNotNull(deployment);
            }
        }
    }
}

