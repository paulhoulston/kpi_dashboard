using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentExistenceChecker : DeploymentDeletionService.ICheckIfDeploymentsExist
    {
        public void DeploymentExists(int deploymentId, Action<DeploymentDeletionService.Deployment> onDeploymentFound)
        {
            var deployment = new DbWrapper().GetSingle<DeploymentDeletionService.Deployment>(SqlQueries.Queries[SqlQueries.Query.GetDeploymentById], new { deploymentId });
            if (deployment != null)
            {
                onDeploymentFound(deployment);
            }
        }
    }
}
