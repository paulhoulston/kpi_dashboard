using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_a_user
    {
        public class WHEN_I_supply_a_null_value
        {
            [Test]
            public void THEN_the_user_is_not_created()
            {
                var userCreated = true;

                new UserCreationService (() => userCreated = false).Create (null);

                Assert.IsFalse (userCreated);
            }
        }
    }

    public class UserCreationService
    {
        readonly Action onUserNotCreated;

        public UserCreationService (Action onUserNotCreated)
        {
            this.onUserNotCreated = onUserNotCreated;
            
        }

        public class User
        {
        }

        public void Create(User user)
        {
            onUserNotCreated ();
        }
    }
}

