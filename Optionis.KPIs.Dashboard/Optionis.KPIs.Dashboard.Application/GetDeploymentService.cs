using System;

namespace Optionis.KPIs.Dashboard.Application
{
    public class GetDeploymentService
    {
        readonly Action _onDeploymentNotFound;
        readonly Action<Deployment> _onDeploymentFound;
        readonly IGetDeployments _repository;

        public interface IGetDeployments
        {
            void Get(int deploymentId, Action onDeploymentNotFound, Action<Deployment> onDeploymentFound);
        }

        public class Deployment
        {
            public int ReleaseId { get; set; }
            public int DeploymentId { get; set; }
            public DateTime DeploymentDate { get; set; }
            public string Version { get; set; }
            public DeploymentStatus DeploymentStatus { get; set; }
        }

        public GetDeploymentService (IGetDeployments repository, Action onDeploymentNotFound, Action<Deployment> onDeploymentFound)
        {
            _repository = repository;
            _onDeploymentNotFound = onDeploymentNotFound;
            _onDeploymentFound = onDeploymentFound;
        }

        public void Get (int deploymentId)
        {
            _repository.Get (deploymentId, _onDeploymentNotFound, _onDeploymentFound);
        }
    }
}

