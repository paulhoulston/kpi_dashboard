using System.Collections.Generic;
using Optionis.KPIs.DataAccess;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class ApplicationLister : ApplicationListingService.IListApplications
    {
        public IEnumerable<ApplicationListingService.Application> List()
        {
            return new DbWrapper().Get<ApplicationListingService.Application>(SqlQueries.Queries[SqlQueries.Query.GetApplications]);
        }
    }
}
