using System;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard.Adapters
{
    public class UserRetriever : GetUserService.IGetUsers
    {
        public void Get (int userId, Action onUserNotFound, Action<GetUserService.User> onUserFound)
        {
            using (var db = new SqliteWrapper ().Connection ()) {
                var user = db.Table<User> ().Where (r => r.Id == userId).SingleOrDefault ();

                if (user == null) {
                    onUserNotFound ();
                    return;
                }

                onUserFound (new GetUserService.User {
                    Id = user.Id,
                    UserName = user.UserName,
                    Created = user.Created
                });
            }
        }
    }
}