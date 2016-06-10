using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_a_user
    {
        class TestRunner : UserCreationService.ICreateUsers
        {
            public int UserId { get; private set; }
            public bool UserCreated{ get; private set; }
            public UserCreationService.ValidationError ErrorReturned { get; private set; }

            public TestRunner (UserCreationService.User userToCreate)
            {
                new UserCreationService (
                    this,
                    error => ErrorReturned = error,
                    userId => UserId = userId
                ).Create (userToCreate);
            }

            public void Create (UserCreationService.User user, Action<int> onUserCreated)
            {
                UserCreated = true;
            }
        }

        public class WHEN_I_add_a_null_value
        {
            readonly TestRunner _testRunner = new TestRunner(null);

            [Test]
            public void THEN_the_user_is_not_created()
            {
                Assert.IsFalse (_testRunner.UserCreated);
            }

            [Test]
            public void AND_a_user_is_null_error_is_returned()
            {
                Assert.AreEqual (UserCreationService.ValidationError.UserIsNull, _testRunner.ErrorReturned);
            }
        }

        public class WHEN_I_add_a_valid_user
        {
            readonly TestRunner _testRunner = new TestRunner(new UserCreationService.User{
                UserName = "Test User"
            });

            [Test]
            public void THEN_the_user_is_created()
            {
                Assert.IsTrue (_testRunner.UserCreated);
            }
        }

        public class WHEN_I_add_a_user_without_a_user_name
        {
            readonly TestRunner _testRunner = new TestRunner(new UserCreationService.User{
                UserName = null
            });

            [Test]
            public void THEN_the_user_is_not_created()
            {
                Assert.IsFalse (_testRunner.UserCreated);
            }

            [Test]
            public void AND_a_user_name_empty_error_is_returned()
            {
                Assert.AreEqual (UserCreationService.ValidationError.UserNameEmpty, _testRunner.ErrorReturned);
            }
        }

        [TestFixture(true, "01234567890123456789012345678901234567890123456789")]
        [TestFixture(false, "012345678901234567890123456789012345678901234567890")]
        public class WHEN_I_add_a_user_with_a_user_name_exceeding_50_characters
        {
            readonly TestRunner _testRunner;
            readonly bool _isValid;

            public WHEN_I_add_a_user_with_a_user_name_exceeding_50_characters (bool isValid, string userName)
            {
                _isValid = isValid;
                _testRunner = new TestRunner(new UserCreationService.User{
                    UserName = userName
                });
            }

            [Test]
            public void THEN_the_user_is_not_created()
            {
                Assert.AreEqual (_isValid, _testRunner.UserCreated);
            }

            [Test]
            public void AND_a_user_name_too_long_error_is_returned()
            {
                if(_isValid)
                    Assert.AreEqual (UserCreationService.ValidationError.None, _testRunner.ErrorReturned);
                else
                    Assert.AreEqual (UserCreationService.ValidationError.UserNameTooLong, _testRunner.ErrorReturned);
            }
        }
    }
}

