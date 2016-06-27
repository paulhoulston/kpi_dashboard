using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class UserLister : UserListingService.IListUsers
    {
        const string SQL = @"SELECT Id FROM Users";

        public IEnumerable<UserListingService.User> List()
        {
            return new DbWrapper().Get<UserListingService.User>(SQL);
        }
    }
}
