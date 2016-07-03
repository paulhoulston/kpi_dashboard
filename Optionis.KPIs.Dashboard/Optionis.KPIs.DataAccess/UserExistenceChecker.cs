using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class UserExistenceChecker : ReleseCreationService.ICheckUsersExist
    {
        public void UserExists(string userName, Action onUserNotExist, Action onUserExist)
        {
            if (UserDoesNotExist(userName))
                onUserNotExist();
            else
                onUserExist();
        }

        static bool UserDoesNotExist(string userName)
        {
            return new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.CheckUserExists], new { userName }) == 0;
        }
    }
}
