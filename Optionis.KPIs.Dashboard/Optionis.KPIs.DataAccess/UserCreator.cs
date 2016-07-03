using System;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess.Database;

namespace Optionis.KPIs.DataAccess
{
    public class UserCreator : UserCreationService.ICreateUsers
    {
        public void Create(UserCreationService.User user, Action<int> onUserCreated)
        {
            var userId = new DbWrapper().ExecuteScalar(SqlQueries.Queries[SqlQueries.Query.InsertUser], new { created = DateTime.Now, userName = user.UserName });
            onUserCreated(userId);
        }
    }
}
