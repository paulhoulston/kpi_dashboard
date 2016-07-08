using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_update_a_deployment_status
    {
        public class WHEN_I_specify_a_status_for_a_valid_deployment
        {
            bool _statusUpdated;
            readonly DeploymentStatusUpdaterService _service;

            public WHEN_I_specify_a_status_for_a_valid_deployment()
            {
                _service = new DeploymentStatusUpdaterService(() => _statusUpdated = true);
                _service.UpdateService();
            }

            [Test]
            public void THEN_the_status_is_updated()
            {
                Assert.IsTrue(_statusUpdated);
            }
        }
    }

    public class DeploymentStatusUpdaterService
    {
        readonly Action _onStatusUpdated;

        public DeploymentStatusUpdaterService(Action onStatusUpdated)
        {
            _onStatusUpdated= onStatusUpdated
        }

        public void UpdateService()
        {
            _onStatusUpdated();
        }
    }
}
