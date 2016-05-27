using System;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard.Adapters
{
    public class ReleaseRetriever : GetReleaseService.IGetReleases
    {
        public void Get (int releaseId, Action onReleaseNotFound, Action<GetReleaseService.Release> onReleaseFound)
        {
            using (var db = new SqliteWrapper ().Connection ()) {
                var release = db.Table<Release> ().Where (r => r.Id == releaseId).SingleOrDefault ();

                if (release == null) {
                    onReleaseNotFound ();
                    return;
                }

                onReleaseFound (new GetReleaseService.Release {
                    Id = release.Id,
                    Application = release.Application,
                    Comments = release.Comments,
                    Created = release.Created,
                    CreatedBy = release.CreatedBy,
                    Title = release.Title,
                    IssueIds = db.Table<Issue>().Where(i => i.ReleaseId == releaseId).ToArray().Select(i => i.Id),
                    DeploymentIds = db.Table<Deployment>().Where(d => d.ReleaseId == releaseId).ToArray().Select(d => d.Id)
                });
            }
        }
    }
}
