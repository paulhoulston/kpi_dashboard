﻿using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class DeploymentCreator : DeploymentCreationService.ICreateDeployments
    {
        public void CreateDeployment(DeploymentCreationService.Deployment deployment, Action<int> onDeploymentCreated)
        {
            var deploymentId = new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.InsertDeployment], new
            {
                releaseId = deployment.ReleaseId,
                deploymentDate = deployment.DeploymentDate,
                deploymentStatus = deployment.Status,
                version = deployment.Version,
                comments = deployment.Comments
            });
            onDeploymentCreated(deploymentId);
        }
    }
}
