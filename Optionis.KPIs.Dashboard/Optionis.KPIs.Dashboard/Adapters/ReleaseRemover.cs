using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;
using System.Linq;

namespace Optionis.KPIs.Dashboard
{

    public class ReleaseRemover : DeploymentDeletionService.IDeleteReleases
    {
        public void DeleteRelease(int releaseId)
        {
            using (var connection = new SqliteWrapper ().Connection ()) {
                var release = connection.Get<Release> (releaseId);
                connection.Delete (release);
            }
        }
    }

}
