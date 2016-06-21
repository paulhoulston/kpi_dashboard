using System;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard.Adapters
{
    public class DeploymentRetriever : GetDeploymentService.IGetDeployments
    {
        public void Get (int deploymentId, Action onDeploymentNotFound, Action<GetDeploymentService.Deployment> onDeploymentFound)
        {
            using (var db = new SqliteWrapper ().Connection ()) {
                var deployment = db.Table<Deployment> ().Where (r => r.Id == deploymentId).SingleOrDefault ();

                if (deployment == null) {
                    onDeploymentNotFound ();
                    return;
                }

                onDeploymentFound (new GetDeploymentService.Deployment {
                    DeploymentId = deployment.Id,
                    ReleaseId = deployment.ReleaseId,
                    DeploymentDate = deployment.DeploymentDate,
                    Version = deployment.Version,
                    Status = (DeploymentStatus)Enum.Parse(typeof(DeploymentStatus), deployment.DeploymentStaus)
                });
            }
        }
    }
}
