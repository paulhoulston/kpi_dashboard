using System;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard
{
    public class UserRepository : ReleseCreationService.ICheckUsersExist
    {
        public void UserExists (int userId, Action onUserNotExist, Action onUserExist)
        {
            using (var connection = new SqliteWrapper ().Connection ()) {
                var user = 
                    connection
                    .Query<User> ("SELECT * FROM User WHERE Id = @userId", new { userId})
                        .SingleOrDefault ();
                if (user == null)
                    onUserNotExist ();
                else
                    onUserExist ();
            }
        }
    }
}

