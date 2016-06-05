using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_a_user
    {
        public class WHEN_I_add_a_null_value
        {
            [Test]
            public void THEN_the_user_is_not_created()
            {
                var userCreated = true;

                new UserCreationService (
                    () => userCreated = false,
                    () => { throw new Exception("Shouldn't hit this"); }
                ).Create (null);

                Assert.IsFalse (userCreated);
            }
        }

        public class WHEN_I_add_a_valid_user
        {
            [Test]
            public void THEN_the_user_is_created()
            {
                var userCreated = false;

                new UserCreationService (
                    () => { throw new Exception("Shouldn't hit this"); },
                    () => userCreated = true
                ).Create (new UserCreationService.User{
                    UserName = "Test User"
                });

                Assert.IsTrue (userCreated);
            }
        }

        public class WHEN_I_add_a_user_without_a_user_name
        {
            [Test]
            public void THEN_the_user_is_not_created()
            {
                var userCreated = true;

                new UserCreationService (
                    () => userCreated = false,
                    () => { throw new Exception ("Shouldn't hit this"); }
                ).Create (new UserCreationService.User {
                    UserName = null
                });

                Assert.IsFalse (userCreated);
            }
        }
    }

    public class UserCreationService
    {
        readonly Action onUserNotCreated;
        readonly Action onUserCreated;

        public UserCreationService (Action onUserNotCreated, Action onUserCreated)
        {
            this.onUserCreated = onUserCreated;
            this.onUserNotCreated = onUserNotCreated;
            
        }

        public class User
        {
            public string UserName { get; set; }
        }

        public void Create(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName))
                onUserNotCreated ();
            else
                onUserCreated ();
        }
    }
}

