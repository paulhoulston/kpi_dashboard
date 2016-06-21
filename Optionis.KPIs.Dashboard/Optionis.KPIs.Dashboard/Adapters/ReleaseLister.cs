using System.Collections.Generic;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard.Adapters
{
    public class ReleaseLister : ReleaseListingService.IListReleases
    {
        public IEnumerable<ReleaseListingService.Release> List (int top)
        {
            using (var connection = new SqliteWrapper ().Connection ()) {
                return connection
                    .Table<Release> ()
                    .ToArray ()
                    .OrderByDescending (release => release.Created)
                    .Take (top)
                    .Select (releaseId => new ReleaseListingService.Release{ ReleaseId = releaseId.Id });
            }
        }
    }

}

