using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_a_user
    {
        public class WHEN_I_add_a_null_value
        {
            bool _userCreated = true;
            UserCreationService.Error _errorReturned;

            public WHEN_I_add_a_null_value ()
            {
                new UserCreationService (
                    error => {
                        _userCreated = false;
                        _errorReturned = error;
                    },
                    () => { throw new Exception("Shouldn't hit this"); }
                ).Create (null);
            }

            [Test]
            public void THEN_the_user_is_not_created()
            {
                Assert.IsFalse (_userCreated);
            }

            [Test]
            public void AND_a_user_is_null_error_is_returned()
            {
                Assert.AreEqual (UserCreationService.Error.UserIsNull, _errorReturned);
            }
        }

        public class WHEN_I_add_a_valid_user
        {
            [Test]
            public void THEN_the_user_is_created()
            {
                var userCreated = false;

                new UserCreationService (
                    _ => { throw new Exception("Shouldn't hit this"); },
                    () => userCreated = true
                ).Create (new UserCreationService.User{
                    UserName = "Test User"
                });

                Assert.IsTrue (userCreated);
            }
        }

        public class WHEN_I_add_a_user_without_a_user_name
        {
            bool _userCreated;
            UserCreationService.Error _errorReturned;

            public WHEN_I_add_a_user_without_a_user_name ()
            {
                new UserCreationService (
                    error => {
                        _userCreated = false;
                        _errorReturned = error;
                    },
                    () => { throw new Exception ("Shouldn't hit this"); }
                ).Create (new UserCreationService.User {
                    UserName = null
                });
                   
            }

            [Test]
            public void THEN_the_user_is_not_created()
            {
                Assert.IsFalse (_userCreated);
            }

            [Test]
            public void AND_a_user_name_empty_error_is_returned()
            {
                Assert.AreEqual (UserCreationService.Error.UserNameEmpty, _errorReturned);
            }
        }
    }
}

