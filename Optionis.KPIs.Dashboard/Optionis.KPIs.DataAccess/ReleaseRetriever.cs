using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseRetriever : GetReleaseService.IGetReleases
    {
        const string SQL =
            "SELECT r.Id, r.Application, r.Comments, r.Created, r.CreatedBy, r.Title, i.Id AS r.IssueIds, d.Id AS DeploymentsIds " +
            "FROM Releases r " +
            "JOIN Issues i ON i.ReleaseId = r.Id " +
            "JOIN Deployments d ON d.ReleaseId = r.Id " +
            "WHERE r.Id = @releaseId";

        public void Get(int releaseId, Action onReleaseNotFound, Action<GetReleaseService.Release> onReleaseFound)
        {
            var release = new DbWrapper().GetSingle<GetReleaseService.Release>(SQL, new { releaseId });
            if (release == null)
                onReleaseNotFound();
            else
                onReleaseFound(release);
        }
    }
}
