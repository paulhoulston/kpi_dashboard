using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;
using System.Linq;

namespace Optionis.KPIs.Dashboard
{

    public class DeploymentRemover : DeploymentDeletionService.IDeleteDeployments
    {
        public void DeleteDeployment(int deploymentId, Action onDeploymentDeleted)
        {
            using (var connection = new SqliteWrapper ().Connection ()) {
                var deployment = connection.Get<Deployment> (deploymentId);
                connection.Delete (deployment);
            }
            onDeploymentDeleted ();
        }
    }

}
