using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_a_release
    {
        class TestRunner : ReleseCreationService.ICreateReleases, ReleseCreationService.ICheckUsersExist
        {
            readonly bool _userExists;
            public bool ReleaseCreated { get; private set;}
            public DateTime CreatedDate{ get;private set;}
            public ReleseCreationService.ValidationError ValidationError { get; private set; }

            public void Create (ReleseCreationService.ReleaseToCreate model, Action<int> onReleaseCreated)
            {
                onReleaseCreated (-1);
            }

            public void UserExists (string userName, Action onUserNotExist, Action onUserExist)
            {
                if (_userExists)
                    onUserExist ();
                else
                    onUserNotExist ();
            }

            public TestRunner (ReleseCreationService.ReleaseToCreate release, bool userExists = true)
            {
                _userExists = userExists;
                new ReleseCreationService(
                    this,
                    this,
                    error => ValidationError = error,
                    _ => ReleaseCreated = true).Create(release);
            }
        }

        public class WHEN_I_supply_a_valid_model
        {
            readonly TestRunner _testRunner = new TestRunner(new ReleseCreationService.ReleaseToCreate{
                Version = "2.16.69.0",
                Title = "Test release",
                Application = "Test application",
                CreatedBy = "Paul Houlston",
                DeploymentDate = DateTime.Today.AddDays(-3)
            });

            [Test]
            public void THEN_the_release_is_created()
            {
                Assert.True (_testRunner.ReleaseCreated);
            }
                
            [Test]
            public void AND_no_validation_message_is_returned(){
                Assert.AreEqual (ReleseCreationService.ValidationError.None, _testRunner.ValidationError);
            }
        }

        public class WHEN_the_creation_model_is_null
        {
            readonly TestRunner _testRunner = new TestRunner(null);

            [Test]
            public void THEN_the_release_is_not_created()
            {
                Assert.False (_testRunner.ReleaseCreated);
            }

            [Test]
            public void AND_a_validation_message_is_returned(){
                Assert.AreEqual (ReleseCreationService.ValidationError.ObjectNotSet, _testRunner.ValidationError);
            }
        }

        [TestFixture(null)]
        [TestFixture("")]
        [TestFixture("1.")]
        [TestFixture("1.2")]
        [TestFixture("1.2.3")]
        [TestFixture("1.2.3.a")]
        [TestFixture("RGERsdfG")]
        public class WHEN_the_creation_model_has_an_invalid_version
        {
            readonly TestRunner _testRunner;

            public WHEN_the_creation_model_has_an_invalid_version (string version)
            {
                _testRunner = new TestRunner (new ReleseCreationService.ReleaseToCreate {
                    Version = version,
                    Title = "Test release",
                    Application = "Test application",
                    CreatedBy = "Paul Houlston",
                    DeploymentDate = DateTime.Today.AddDays(-3)
                });
            }

            [Test]
            public void THEN_the_release_is_not_created()
            {
                Assert.False (_testRunner.ReleaseCreated);
            }

            [Test]
            public void AND_a_validation_message_is_returned()
            {
                Assert.AreEqual (ReleseCreationService.ValidationError.InvalidVersion, _testRunner.ValidationError);
            }
        }

        public class WHEN_the_creation_model_does_not_have_a_title_set
        {
            readonly TestRunner _testRunner = new TestRunner (new ReleseCreationService.ReleaseToCreate {
                Version = "1.0.0.*",
                Title = string.Empty,
                Application = "Test application",
                CreatedBy = "Paul Houlston",
                DeploymentDate = DateTime.Today.AddDays(-3)
            });

            [Test]
            public void THEN_the_release_is_not_created()
            {
                Assert.False (_testRunner.ReleaseCreated);
            }

            [Test]
            public void AND_a_validation_message_is_returned()
            {
                Assert.AreEqual (ReleseCreationService.ValidationError.TitleNotSet, _testRunner.ValidationError);
            }
        }

        public class WHEN_the_creation_model_does_not_have_an_application_set
        {
            readonly TestRunner _testRunner = new TestRunner(new ReleseCreationService.ReleaseToCreate {
                Version = "1.0.0.*",
                Title = "Test release",
                Application = string.Empty,
                CreatedBy = "Paul Houlston",
                DeploymentDate = DateTime.Today.AddDays(-3)
            });

            [Test]
            public void THEN_the_release_is_not_created()
            {
                Assert.False (_testRunner.ReleaseCreated);
            }

            [Test]
            public void AND_a_validation_message_is_returned()
            {
                Assert.AreEqual (ReleseCreationService.ValidationError.ApplicationNotSet, _testRunner.ValidationError);
            }
        }

        public class WHEN_the_creation_model_does_not_have_a_valid_created_by_user
        {
            readonly TestRunner _testRunner = new TestRunner(new ReleseCreationService.ReleaseToCreate {
                Version = "1.0.0.*",
                Title = "Test release",
                Application = "Test application",
                CreatedBy = "Paul Houlston",
                DeploymentDate = DateTime.Today.AddDays(-3)
            }, false);

            [Test]
            public void THEN_the_release_is_not_created()
            {
                Assert.False (_testRunner.ReleaseCreated);
            }

            [Test]
            public void AND_a_validation_message_is_returned()
            {
                Assert.AreEqual (ReleseCreationService.ValidationError.UserNotFound, _testRunner.ValidationError);
            }
        }

        [TestFixture(true,  "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891234")]
        [TestFixture(true,  "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678912345")]
        [TestFixture(false, "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789123456")]
        public class WHEN_the_creation_model_has_comments_exceeding_255_characters
        {
            readonly TestRunner _testRunner;
            readonly bool _isValid;

            public WHEN_the_creation_model_has_comments_exceeding_255_characters (bool isValid, string comments)
            {
                _isValid = isValid;
                _testRunner = new TestRunner(new ReleseCreationService.ReleaseToCreate {
                    Version = "1.0.0.*",
                    Title = "Test release",
                    Application = "Test application",
                    CreatedBy = "Paul Houlston",
                    Comments = comments,
                    DeploymentDate = DateTime.Today.AddDays(-3)
                });
            }

            [Test]
            public void THEN_the_release_is_not_created()
            {
                Assert.AreEqual(_isValid, _testRunner.ReleaseCreated);
            }

            [Test]
            public void AND_a_validation_message_is_returned()
            {
                if(_isValid)
                    Assert.AreEqual (ReleseCreationService.ValidationError.None, _testRunner.ValidationError);
                else
                    Assert.AreEqual (ReleseCreationService.ValidationError.InvalidComments, _testRunner.ValidationError);
            }
        }

        public class WHEN_the_creation_model_has_a_deployment_date_older_than_30_days
        {
            readonly TestRunner _testRunner = new TestRunner(new ReleseCreationService.ReleaseToCreate {
                Version = "1.0.0.*",
                Title = "Test release",
                Application = "Test application",
                CreatedBy = "Paul Houlston",
                DeploymentDate = DateTime.Today.AddDays(-30).AddMilliseconds(-1)
            });

            [Test]
            public void THEN_the_release_is_not_created()
            {
                Assert.False (_testRunner.ReleaseCreated);
            }

            [Test]
            public void AND_a_validation_message_is_returned()
            {
                Assert.AreEqual (ReleseCreationService.ValidationError.InvalidDeploymentDate, _testRunner.ValidationError);
            }
        }

    }
}
