using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class UserRetriever : GetUserService.IGetUsers
    {
        const string SQL = @"SELECT Id, UserName, Created FROM Users WHERE Id = @userId ORDER BY UserName";

        public void Get(int userId, Action onUserNotFound, Action<GetUserService.User> onUserFound)
        {
            var user = new DbWrapper().GetSingle<GetUserService.User>(SQL, new { userId });
            if (user == null)
                onUserNotFound();
            else
                onUserFound(user);
        }
    }
}