using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentRemover : DeploymentDeletionService.IDeleteDeployments
    {
        const string SQL = @"DELETE FROM Deployments WHERE Id = @deploymentId";

        public void DeleteDeployment(int deploymentId, Action onDeploymentDeleted)
        {
            new DbWrapper().ExecuteScalar(SQL, new { deploymentId });
            onDeploymentDeleted();
        }
    }
}