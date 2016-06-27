using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseRemover : DeploymentDeletionService.IDeleteReleases
    {
        const string SQL = @"DELETE FROM Releases WHERE Id = @releaseId";

        public void DeleteRelease(int releaseId)
        {
            new DbWrapper().ExecuteScalar(SQL, new { releaseId });
        }
    }
}
