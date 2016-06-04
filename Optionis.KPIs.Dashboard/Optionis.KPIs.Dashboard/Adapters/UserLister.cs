using System.Collections.Generic;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard.Adapters
{
    public class UserLister : UserListingService.IListUsers
    {
        public IEnumerable<UserListingService.User> List ()
        {
            using (var connection = new SqliteWrapper ().Connection ()) {
                return connection
                    .Table<User> ()
                    .ToArray ()
                    .Select (userId => new UserListingService.User(userId.Id));
            }
        }
    }

}

