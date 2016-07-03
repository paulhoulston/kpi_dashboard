using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class UserRetriever : GetUserService.IGetUsers
    {
        public void Get(int userId, Action onUserNotFound, Action<GetUserService.User> onUserFound)
        {
            var user = new DbWrapper().GetSingle<GetUserService.User>(SqlQueries.Queries[SqlQueries.Query.GetUserById], new { userId });
            if (user == null)
                onUserNotFound();
            else
                onUserFound(user);
        }
    }
}