using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentRetriever : GetDeploymentService.IGetDeployments
    {
        public void Get(int deploymentId, Action onDeploymentNotFound, Action<GetDeploymentService.Deployment> onDeploymentFound)
        {
            var deployment = new DbWrapper().GetSingle<GetDeploymentService.Deployment>(SqlQueries.Queries[SqlQueries.Query.GetDeploymentById], new { deploymentId });

            if (deployment == null)
                onDeploymentNotFound();
            else
                onDeploymentFound(deployment);
        }
    }
}
