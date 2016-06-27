using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseHasAssignedDeploymentsChecker : DeploymentDeletionService.ICheckIfReleaseHasDeployments
    {
        const string SQL = @"SELECT COUNT(ReleaseId) FROM Deployments WHERE ReleaseId = @releaseId";

        public void DeploymentsAssigned(int releaseId, Action onNoDeploymentsAssigned)
        {
            if (ReleaseHasNoDeployments(releaseId))
            {
                onNoDeploymentsAssigned();
            }
        }

        static bool ReleaseHasNoDeployments(int releaseId)
        {
            return new DbWrapper().ExecuteScalar(SQL, new { releaseId }) == 0;
        }
    }
}
