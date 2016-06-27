using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentRetriever : GetDeploymentService.IGetDeployments
    {
        const string SQL = @"SELECT Id AS DeploymentId, ReleaseId, DeploymentDate, Version, DeploymentStatus FROM Deployments WHERE Id = @deploymentId";

        public void Get(int deploymentId, Action onDeploymentNotFound, Action<GetDeploymentService.Deployment> onDeploymentFound)
        {
            var deployment = new DbWrapper().GetSingle<GetDeploymentService.Deployment>(SQL, new { deploymentId });

            if (deployment == null)
                onDeploymentNotFound();
            else
                onDeploymentFound(deployment);
        }
    }
}
