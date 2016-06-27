using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class UserExistenceChecker : ReleseCreationService.ICheckUsersExist
    {
        const string SQL = @"SELECT COUNT(*) FROM Users WHERE UserName = '@userName'";

        public void UserExists(string userName, Action onUserNotExist, Action onUserExist)
        {
            if (UserDoesNotExist(userName))
                onUserNotExist();
            else
                onUserExist();
        }

        static bool UserDoesNotExist(string userName)
        {
            return new DbWrapper().ExecuteScalar(SQL, new { userName }) == 0;
        }
    }
}
