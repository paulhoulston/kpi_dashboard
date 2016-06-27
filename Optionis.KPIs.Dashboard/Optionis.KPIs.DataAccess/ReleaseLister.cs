using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class ReleaseLister : ReleaseListingService.IListReleases
    {
        const string SQL = "SELECT TOP {=top} Id AS ReleaseId FROM Releases ORDER BY Created DESC";

        public IEnumerable<ReleaseListingService.Release> List(int top)
        {
            return new DbWrapper().Get<ReleaseListingService.Release>(SQL, new { top });
        }
    }
}
