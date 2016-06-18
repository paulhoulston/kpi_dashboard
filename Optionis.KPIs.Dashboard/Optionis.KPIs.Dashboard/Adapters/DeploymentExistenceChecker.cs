using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard
{
    public class DeploymentExistenceChecker : DeploymentDeletionService.ICheckIfDeploymentsExist
    {
        public void DeploymentExists(int deploymentId, Action<DeploymentDeletionService.Deployment> onDeploymentFound)
        {
            Deployment deployment;
            using (var connection = new SqliteWrapper ().Connection ()) {
                deployment = connection.Find<Deployment> (deploymentId);
            }

            if (deployment != null)
                onDeploymentFound (new DeploymentDeletionService.Deployment {
                    DeploymentId = deploymentId,
                    ReleaseId = deployment.ReleaseId
                });
        }
    }
}

