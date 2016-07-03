using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseLister : ReleaseListingService.IListReleases
    {
        public IEnumerable<ReleaseListingService.Release> List(int top)
        {
            return new DbWrapper().Get<ReleaseListingService.Release>(SqlQueries.Queries[SqlQueries.Query.GetTopXReleases], new { top });
        }
    }
}
