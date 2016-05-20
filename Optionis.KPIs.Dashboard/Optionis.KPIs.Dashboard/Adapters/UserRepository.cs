using System;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard
{
    public class UserRepository : ReleseCreationService.ICheckUsersExist
    {
        public void UserExists (string userName, Action onUserNotExist, Action onUserExist)
        {
            if (GetUser (userName) == null)
                onUserNotExist ();
            else
                onUserExist ();
        }

        static User GetUser (string userName)
        {
            User user;
            using (var cnn = new SqliteWrapper ().Connection ()) {
                user = cnn.Table<User> ().Where (usr => usr.UserName.Equals (userName)).SingleOrDefault ();
            }
            return user;
        }
    }
}

