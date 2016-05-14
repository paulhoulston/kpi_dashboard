using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    [TestFixture]
    public class GIVEN_I_want_to_create_a_release
    {
        public class WHEN_I_do_supply_a_valid_model : ReleseCreationService.ICreateReleases
        {
            bool _releaseCreated;
            DateTime _createdDate;
            readonly DateTime _startTime;
            ReleseCreationService.ValidationError? _validationError;

            public void Create (ReleseCreationService.IAmARelease model)
            {
                _releaseCreated = true;
                _createdDate = model.Created;
            }

            public WHEN_I_do_supply_a_valid_model ()
            {
                _startTime = DateTime.Now;
                new ReleseCreationService(this, error => _validationError = error).Create(new ReleseCreationService.ReleaseToCreate{
                    Version = "2.16.69.0"
                });
            }

            [Test]
            public void THEN_the_release_is_created()
            {
                Assert.True (_releaseCreated);
            }

            [Test]
            public void AND_the_creation_date_is_set_to_now()
            {
                Assert.IsTrue (_createdDate >= _startTime);
            }

            [Test]
            public void AND_no_validation_message_is_returned(){
                Assert.IsNull (_validationError);
            }
        }

        public class WHEN_the_creation_model_is_null : ReleseCreationService.ICreateReleases
        {
            bool _releaseCreated;
            ReleseCreationService.ValidationError _validationError;

            public void Create (ReleseCreationService.IAmARelease model)
            {
                _releaseCreated = true;
            }

            [Test]
            public void THEN_the_release_is_not_created()
            {
                new ReleseCreationService(this, error => _validationError = error).Create(null);
                Assert.False (_releaseCreated);
            }
        }

        public class WHEN_the_creation_model_has_an_invalid_version :  ReleseCreationService.ICreateReleases
        {
            bool _releaseCreated;
            ReleseCreationService.ValidationError _validationMessage;
            readonly ReleseCreationService _svc;

            public void Create (ReleseCreationService.IAmARelease model)
            {
                _releaseCreated = true;
            }

            public WHEN_the_creation_model_has_an_invalid_version ()
            {
                _svc = new ReleseCreationService(this, error => _validationMessage = error);
            }

            [TestCase("")]
            [TestCase("1.")]
            [TestCase("1.2")]
            [TestCase("1.2.3")]
            [TestCase("1.2.3.a")]
            [TestCase("RGERsdfG")]
            public void THEN_the_release_is_not_created(string version)
            {
                _svc.Create (new ReleseCreationService.ReleaseToCreate {
                    Version = version
                });
                Assert.False (_releaseCreated);
            }

            [TestCase("")]
            [TestCase("1.")]
            [TestCase("1.2")]
            [TestCase("1.2.3")]
            [TestCase("1.2.3.a")]
            [TestCase("RGERsdfG")]
            public void AND_a_validation_message_is_returned(string version)
            {
                _svc.Create (new ReleseCreationService.ReleaseToCreate {
                    Version = version
                });
                Assert.AreEqual (ReleseCreationService.ValidationError.InvalidVersion, _validationMessage);
            }
        }
    }

    public class ReleseCreationService
    {
        readonly ICreateReleases _repository;
        readonly Action<ValidationError> _onValidationError;
        public enum ValidationError
        {
            InvalidVersion = 0
        }

        public class ReleaseToCreate
        {
            public string Version{get;set;}
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
            public DateTime Created {get;set;}
            public string Version{get;set;}
        }

        public ReleseCreationService (ICreateReleases repository, Action<ValidationError> onValidationError)
        {
            _onValidationError = onValidationError;
            _repository = repository;
        }

        public void Create (ReleaseToCreate release)
        {
            if (release == null ||
                IsInvalidVersion(release.Version))
                return;
            
            
            _repository.Create (new RepositoryRelease {
                Version = release.Version,
                Created = DateTime.Now
            });
        }

        static bool IsInvalidVersion(string version)
        {
            const string regex = @"^\d+[.]\d+[.]\d+[.](\d+|\*)$";
            var isMatch = new Regex (regex).IsMatch (version);
            return !isMatch;
        }
    }
}

