using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class UserLister : UserListingService.IListUsers
    {
        public IEnumerable<UserListingService.User> List()
        {
            return new DbWrapper().Get<UserListingService.User>(SqlQueries.Queries[SqlQueries.Query.GetUserIds]);
        }
    }
}
