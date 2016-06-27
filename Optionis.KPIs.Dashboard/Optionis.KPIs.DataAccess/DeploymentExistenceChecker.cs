using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentExistenceChecker : DeploymentDeletionService.ICheckIfDeploymentsExist
    {
        const string SQL = @"SELECT Id AS DeploymentId, ReleaseId FROM Deployments WHERE Id = @deploymentId";

        public void DeploymentExists(int deploymentId, Action<DeploymentDeletionService.Deployment> onDeploymentFound)
        {
            var deployment = new DbWrapper().GetSingle<DeploymentDeletionService.Deployment>(SQL, new { deploymentId });
            if (deployment != null)
            {
                onDeploymentFound(deployment);
            }
        }
    }
}
