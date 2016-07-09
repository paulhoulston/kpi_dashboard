using System;
using Optionis.KPIs.Dashboard.Application.Interfaces;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentExistenceChecker : ICheckIfDeploymentsExist
    {
        public void DeploymentExists(int deploymentId, Action<Deployment> onDeploymentFound)
        {
            var deployment = new DbWrapper().GetSingle<Deployment>(SqlQueries.Queries[SqlQueries.Query.GetDeploymentById], new { deploymentId });
            if (deployment != null)
            {
                onDeploymentFound(deployment);
            }
        }
    }
}
