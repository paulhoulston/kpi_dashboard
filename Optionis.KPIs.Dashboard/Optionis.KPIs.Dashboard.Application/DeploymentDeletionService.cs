using System;

namespace Optionis.KPIs.Dashboard.Application
{
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