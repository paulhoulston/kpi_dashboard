using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    [TestFixture]
    public class GIVEN_I_want_to_create_a_release
    {
        class TestRunner : ReleseCreationService.ICreateReleases
        {
            public bool ReleaseCreated { get; private set;}
            public DateTime CreatedDate{ get;private set;}
            public ReleseCreationService.ValidationError ValidationError { get; private set; }

            public void Create (ReleseCreationService.IAmARelease model)
            {
                ReleaseCreated = true;
                CreatedDate = model.Created;
            }

            public TestRunner (ReleseCreationService.ReleaseToCreate release)
            {
                new ReleseCreationService(this, error => ValidationError = error).Create(release);
            }
        }

        public class WHEN_I_do_supply_a_valid_model
        {
            readonly DateTime _startTime = DateTime.Now;
            readonly TestRunner _testRunner = new TestRunner(new ReleseCreationService.ReleaseToCreate{
                Version = "2.16.69.0",
                Title = "Test release"
            });

            [Test]
            public void THEN_the_release_is_created()
            {
                Assert.True (_testRunner.ReleaseCreated);
            }

            [Test]
            public void AND_the_creation_date_is_set_to_now()
            {
                Assert.IsTrue (_testRunner.CreatedDate >= _startTime);
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

        public class WHEN_the_creation_model_has_an_invalid_version
        {
            [TestCase("")]
            [TestCase("1.")]
            [TestCase("1.2")]
            [TestCase("1.2.3")]
            [TestCase("1.2.3.a")]
            [TestCase("RGERsdfG")]
            public void THEN_the_release_is_not_created(string version)
            {
                Assert.False (new TestRunner (new ReleseCreationService.ReleaseToCreate {
                    Version = version,
                    Title = "Test release"
                }).ReleaseCreated);
            }

            [TestCase("")]
            [TestCase("1.")]
            [TestCase("1.2")]
            [TestCase("1.2.3")]
            [TestCase("1.2.3.a")]
            [TestCase("RGERsdfG")]
            public void AND_a_validation_message_is_returned(string version)
            {
                Assert.AreEqual (ReleseCreationService.ValidationError.InvalidVersion, 
                    new TestRunner (new ReleseCreationService.ReleaseToCreate {
                        Version = version,
                        Title = "Test release"
                    }).ValidationError);
            }
        }

        public class WHEN_the_creation_model_does_not_have_a_title_set
        {
            readonly TestRunner _testRunner = new TestRunner(new ReleseCreationService.ReleaseToCreate {
                Version = "1.0.0.*",
                Title = string.Empty
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
    }

    public class ReleseCreationService
    {
        readonly ICreateReleases _repository;
        readonly Action<ValidationError> _onValidationError;

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            ObjectNotSet = 1,
            InvalidVersion = 2,
            TitleNotSet = 3
        }

        public class ReleaseToCreate
        {
            public string Version{ get; set; }
            public string Title{ get; set; }
        }

        public interface ICreateReleases
        {
            void Create (IAmARelease model);
        }

        public interface IAmARelease
        {
            DateTime Created{ get; set; }
            string Version{get;set;}
        }

        class RepositoryRelease : IAmARelease
        {
            public DateTime Created { get; set; }
            public string Version{ get; set; }
        }

        public ReleseCreationService (ICreateReleases repository, Action<ValidationError> onValidationError)
        {
            _onValidationError = onValidationError;
            _repository = repository;
        }

        public void Create (ReleaseToCreate release)
        {
            if (ModelIsValid (release))
                _repository.Create (new RepositoryRelease {
                    Version = release.Version,
                    Created = DateTime.Now
                });
        }

        bool ModelIsValid(ReleaseToCreate release){
            foreach (var validationMethod in ValidationMethods) {
                if (validationMethod.Value (release)) {
                    _onValidationError (validationMethod.Key);
                    return false;
                }
            }
            return true;
        }

        static IDictionary<ValidationError, Func<ReleaseToCreate, bool>> ValidationMethods
        {
            get{
                return new Dictionary<ValidationError, Func<ReleaseToCreate, bool>> {
                    { ValidationError.ObjectNotSet, ReleaseIsNull },
                    { ValidationError.TitleNotSet, TitleNotSet },
                    { ValidationError.InvalidVersion, IsInvalidVersion }
                };
            }
        }

        static bool ReleaseIsNull (ReleaseToCreate release)
        {
            return release == null;
        }

        static bool TitleNotSet(ReleaseToCreate release)
        {
            return string.IsNullOrEmpty (release.Title);
        }

        static bool IsInvalidVersion(ReleaseToCreate release)
        {
            const string regex = @"^\d+[.]\d+[.]\d+[.](\d+|\*)$";
            return !new Regex (regex).IsMatch (release.Version);
        }
    }
}
