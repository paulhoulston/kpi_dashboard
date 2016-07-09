using System;

namespace Optionis.KPIs.Dashboard.Application
{
    public class DeploymentStatusUpdaterService
    {
        readonly Action _onStatusUpdated;
        readonly Action _statusNotUpdated;
        readonly ICheckIfDeploymentsExist _repository;
        readonly IUpdateDeploymentStatuses _updater;

        public interface ICheckIfDeploymentsExist
        {
            void IfDeploymentExists(int deploymentId, Action onDeploymentExists, Action onDeploymentNotExists);
        }

        public interface IUpdateDeploymentStatuses
        {
            void Update(int deploymentId, DeploymentStatus status, Action onDeploymentUpdated);
        }

        public DeploymentStatusUpdaterService(
            Action statusNotUpdated,
            Action onStatusUpdated,
            ICheckIfDeploymentsExist repository,
            IUpdateDeploymentStatuses updater)
        {
            _onStatusUpdated = onStatusUpdated;
            _statusNotUpdated = statusNotUpdated;
            _repository = repository;
            _updater = updater;
        }

        public void UpdateStatus(int deploymentId, DeploymentStatus status)
        {
            _repository.IfDeploymentExists(
                deploymentId,
                _statusNotUpdated,
                () => _updater.Update(deploymentId, status, _onStatusUpdated));
        }
    }
}