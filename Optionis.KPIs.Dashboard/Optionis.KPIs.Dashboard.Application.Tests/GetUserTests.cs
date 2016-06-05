using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_get_a_user
    {
        public class WHEN_the_user_does_not_exist : GetUserService.IGetUsers
        {
            public void Get (int userId, Action onUserNotFound, Action<GetUserService.User> onUserFound)
            {
                onUserNotFound ();
            }

            [Test]
            public void THEN_the_user_is_not_returned()
            {
                var isNotFound = false;
                GetUserService.User user = null;
                new GetUserService (this, () => isNotFound = true, _ => user = _).Get (0);
                Assert.IsTrue (isNotFound);
                Assert.IsNull (user);
            }
        }

        public class WHEN_the_user_exists : GetUserService.IGetUsers
        {
            public void Get (int userId, Action onUserNotFound, Action<GetUserService.User> onUserFound)
            {
                onUserFound (new GetUserService.User());
            }

            [Test]
            public void THEN_the_user_is_returned()
            {
                var isNotFound = false;
                GetUserService.User user = null;
                new GetUserService (this, () => isNotFound = true, _ => user = _).Get(0);
                Assert.IsFalse (isNotFound);
                Assert.IsNotNull(user);
            }
        }
    }
}

