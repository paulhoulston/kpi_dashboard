using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;
using System.Linq;

namespace Optionis.KPIs.Dashboard
{

    public class ReleaseHasAssignedDeploymentsChecker : DeploymentDeletionService.ICheckIfReleaseHasDeployments
    {
        public void DeploymentsAssigned(int releaseId, Action onNoDeploymentsAssigned)
        {
            if (!HasDeployments(releaseId))
                onNoDeploymentsAssigned ();
        }

        static bool HasDeployments(int releaseId)
        {
            bool hasDeployments;
            using (var connection = new SqliteWrapper ().Connection ()) {
                hasDeployments = connection.Table<Deployment> ().Where (usr => usr.ReleaseId.Equals (releaseId)).Any ();
            }
            return hasDeployments;
        }
    }
}
