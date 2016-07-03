using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentRemover : DeploymentDeletionService.IDeleteDeployments
    {
        public void DeleteDeployment(int deploymentId, Action onDeploymentDeleted)
        {
            new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.DeleteDeploymentById], new { deploymentId });
            onDeploymentDeleted();
        }
    }
}