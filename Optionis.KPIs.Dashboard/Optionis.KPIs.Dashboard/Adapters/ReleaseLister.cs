using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;
using System.Collections.Generic;
using System.Linq;

namespace Optionis.KPIs.Dashboard.Adapters
{
    public class ReleaseLister : RelaseListingService.IListReleases
    {
        public IEnumerable<RelaseListingService.Release> List ()
        {
            using (var connection = new SqliteWrapper ().Connection ()) {
                return connection
                    .Table<Release> ()
                    .ToArray ()
                    .Select (releaseId => new RelaseListingService.Release(releaseId.Id));
            }
        }
    }
}

