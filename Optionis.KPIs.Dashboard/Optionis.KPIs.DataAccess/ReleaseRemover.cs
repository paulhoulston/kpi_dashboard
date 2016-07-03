using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseRemover : DeploymentDeletionService.IDeleteReleases
    {
        public void DeleteRelease(int releaseId)
        {
            new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.DeleteReleaseById], new { releaseId });
        }
    }
}
