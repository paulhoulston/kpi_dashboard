using System;

namespace Optionis.KPIs.Dashboard.Application.Interfaces
{
    public interface ICheckIfDeploymentsExist
    {
        void DeploymentExists(int deploymentId, Action<Deployment> onDeploymentFound);
    }

    public class Deployment
    {
        public int DeploymentId { get; set; }
        public int ReleaseId { get; set; }
    }
}
