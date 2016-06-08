using System;

namespace Optionis.KPIs.Dashboard.Application
{

    public class UserCreationService
    {
        readonly Action<Error> onUserNotCreated;
        readonly Action onUserCreated;

        public UserCreationService (Action<Error> onUserNotCreated, Action onUserCreated)
        {
            this.onUserCreated = onUserCreated;
            this.onUserNotCreated = onUserNotCreated;
        }

        public enum Error
        {
            UserIsNull,
            UserNameEmpty
        }

        public class User
        {
            public string UserName { get; set; }
        }

        public void Create(User user)
        {
            if (user == null)
                onUserNotCreated (Error.UserIsNull);
            else if (string.IsNullOrEmpty(user.UserName))
                onUserNotCreated (Error.UserNameEmpty);
            else
                onUserCreated ();
        }
    }
}
