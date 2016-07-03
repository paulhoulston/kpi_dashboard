using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseHasAssignedDeploymentsChecker : DeploymentDeletionService.ICheckIfReleaseHasDeployments
    {
        public void DeploymentsAssigned(int releaseId, Action onNoDeploymentsAssigned)
        {
            if (ReleaseHasNoDeployments(releaseId))
            {
                onNoDeploymentsAssigned();
            }
        }

        static bool ReleaseHasNoDeployments(int releaseId)
        {
            return new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.CheckReleaseExists], new { releaseId }) == 0;
        }
    }
}
