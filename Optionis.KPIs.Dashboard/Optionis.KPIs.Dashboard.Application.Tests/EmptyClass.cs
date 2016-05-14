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

            public void Create (ReleseCreationService.IAmARelease model)
            {
                _releaseCreated = true;
                _createdDate = model.Created;
            }

            public WHEN_I_do_supply_a_valid_model ()
            {
                _startTime = DateTime.Now;
                new ReleseCreationService(this).Create(new ReleseCreationService.ReleaseToCreate{
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
        }

        public class WHEN_the_creation_model_is_null : ReleseCreationService.ICreateReleases
        {
            bool _releaseCreated;

            public void Create (ReleseCreationService.IAmARelease model)
            {
                _releaseCreated = true;
            }

            [Test]
            public void THEN_the_release_is_not_created()
            {
                new ReleseCreationService(this).Create(null);
                Assert.False (_releaseCreated);
            }
        }

        public class WHEN_the_creation_model_has_an_invalid_version :  ReleseCreationService.ICreateReleases
        {
            bool _releaseCreated;

            public void Create (ReleseCreationService.IAmARelease model)
            {
                _releaseCreated = true;
            }

            [TestCase("")]
            [TestCase("1.")]
            [TestCase("1.2")]
            [TestCase("1.2.3")]
            [TestCase("1.2.3.a")]
            [TestCase("RGERsdfG")]
            public void THEN_the_release_is_not_created(string version)
            {
                new ReleseCreationService(this).Create (new ReleseCreationService.ReleaseToCreate {
                    Version = version
                });
                Assert.False (_releaseCreated);
            }

            [Test, Ignore]
            public void AND_an_appropriate_error_message_is_returned()
            {
                
            }
        }
    }

    public class ReleseCreationService
    {
        readonly ICreateReleases _repository;

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

        public ReleseCreationService (ICreateReleases repository)
        {
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

