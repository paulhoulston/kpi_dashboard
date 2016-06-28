using System;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.DataAccess
{
    public class UserCreator : UserCreationService.ICreateUsers
    {
        const string SQL = @"INSERT INTO Users(Created, UserName) VALUES (@created, @userName); SELECT SCOPE_IDENTITY();";

        public void Create(UserCreationService.User user, Action<int> onUserCreated)
        {
            var userId = new DbWrapper().ExecuteScalar(SQL, new { created = DateTime.Now, userName = user.UserName });
            onUserCreated(userId);
        }
    }
}
