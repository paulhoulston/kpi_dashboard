using System;
using Optionis.KPIs.Dashboard.Application.Interfaces;

namespace Optionis.KPIs.Dashboard.Application
{
    public class DeploymentStatusUpdaterService
    {
        readonly Action _onStatusUpdated;
        readonly ICheckIfDeploymentsExist _repository;
        readonly IUpdateDeploymentStatuses _updater;

        public interface IUpdateDeploymentStatuses
        {
            void Update(int deploymentId, DeploymentStatus status, Action onDeploymentUpdated);
        }

        public DeploymentStatusUpdaterService(
            Action onStatusUpdated,
            ICheckIfDeploymentsExist repository,
            IUpdateDeploymentStatuses updater)
        {
            _onStatusUpdated = onStatusUpdated;
            _repository = repository;
            _updater = updater;
        }

        public void UpdateStatus(int deploymentId, DeploymentStatus status)
        {
            _repository.DeploymentExists(
                deploymentId,
                _ => _updater.Update(deploymentId, status, _onStatusUpdated));
        }
    }
}