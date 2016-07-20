using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_an_application
    {
        class TestRunner : ApplicationCreationService.ICreateApplications
        {
            public int ApplicationId { get; private set; }
            public bool ApplicationCreated { get; private set; }
            public ApplicationCreationService.ValidationError ErrorReturned { get; private set; }

            public TestRunner (ApplicationCreationService.Application applicationToCreate)
            {
                new ApplicationCreationService(
                    this,
                    error => ErrorReturned = error,
                    applicationId => ApplicationId = applicationId
                ).Create (applicationToCreate);
            }

            public void Create (ApplicationCreationService.Application application, Action<int> onApplicationCreated)
            {
                ApplicationCreated = true;
            }
        }

        public class WHEN_I_add_a_null_value
        {
            readonly TestRunner _testRunner = new TestRunner(null);

            [Test]
            public void THEN_the_application_is_not_created()
            {
                Assert.IsFalse (_testRunner.ApplicationCreated);
            }

            [Test]
            public void AND_an_application_is_null_error_is_returned()
            {
                Assert.AreEqual (ApplicationCreationService.ValidationError.ApplicationIsNull, _testRunner.ErrorReturned);
            }
        }

        public class WHEN_I_add_a_valid_application
        {
            readonly TestRunner _testRunner = new TestRunner(new ApplicationCreationService.Application
            {
                ApplicationName = "Test application"
            });

            [Test]
            public void THEN_the_application_is_created()
            {
                Assert.IsTrue (_testRunner.ApplicationCreated);
            }
        }

        public class WHEN_I_add_an_application_without_a_application_name
        {
            readonly TestRunner _testRunner = new TestRunner(new ApplicationCreationService.Application
            {
                ApplicationName = null
            });

            [Test]
            public void THEN_the_application_is_not_created()
            {
                Assert.IsFalse (_testRunner.ApplicationCreated);
            }

            [Test]
            public void AND_an_application_name_empty_error_is_returned()
            {
                Assert.AreEqual (ApplicationCreationService.ValidationError.ApplicationNameNotSet, _testRunner.ErrorReturned);
            }
        }

        [TestFixture(true, "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234")]
        [TestFixture(false, "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345")]
        public class WHEN_I_add_an_application_with_an_application_name_exceeding_255_characters
        {
            readonly TestRunner _testRunner;
            readonly bool _isValid;

            public WHEN_I_add_an_application_with_an_application_name_exceeding_255_characters(bool isValid, string applicationName)
            {
                _isValid = isValid;
                _testRunner = new TestRunner(new ApplicationCreationService.Application
                {
                    ApplicationName = applicationName
                });
            }

            [Test]
            public void THEN_the_application_is_not_created()
            {
                Assert.AreEqual (_isValid, _testRunner.ApplicationCreated);
            }

            [Test]
            public void AND_an_application_name_too_long_error_is_returned()
            {
                if(_isValid)
                    Assert.AreEqual (ApplicationCreationService.ValidationError.None, _testRunner.ErrorReturned);
                else
                    Assert.AreEqual (ApplicationCreationService.ValidationError.ApplicationNameTooLong, _testRunner.ErrorReturned);
            }
        }
    }
}

