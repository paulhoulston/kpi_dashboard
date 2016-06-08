using System;
using System.Linq;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.ReadCache;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;

namespace Optionis.KPIs.Dashboard.Adapters
{

    public class UserCreator : UserCreationService.ICreateUsers
    {
        public void Create (UserCreationService.User user, Action<int> onUserCreated)
        {
            var dbUser = new User {
                Created = DateTime.Now,
                UserName = user.UserName
            };

            using (var connection = new SqliteWrapper ().Connection ()) {
                connection.Insert (dbUser);
            }

            onUserCreated (dbUser.Id);
        }
    }
}