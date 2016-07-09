using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentStatusUpdater : DeploymentStatusUpdaterService.IUpdateDeploymentStatuses
    {
        public void Update(int deploymentId, DeploymentStatus deploymentStatus, Action onDeploymentUpdated)
        {
            new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.UpdateDeploymentStatus], new { deploymentId, deploymentStatus });
            onDeploymentUpdated();
        }
    }
}
