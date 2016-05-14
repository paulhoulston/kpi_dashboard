using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    [TestFixture]
    public class GIVEN_I_want_to_create_a_release
    {
        public class WHEN_I_do_supply_a_valid_model : ReleseCreationService.ICreateReleases
        {
            bool? _releaseCreated = null;
            DateTime _createdDate;
            readonly DateTime _startTime;

            public void Create (ReleseCreationService.ReleaseToCreate model)
            {
                _createdDate = model.Created;
            }

            public WHEN_I_do_supply_a_valid_model ()
            {
                _startTime = DateTime.Now;
                new ReleseCreationService(this, () => _releaseCreated = false, () => _releaseCreated = true).Create(new ReleseCreationService.ReleaseToCreate());
            }

            [Test]
            public void THEN_the_release_is_created()
            {
                Assert.True (_releaseCreated.Value);
            }

            [Test]
            public void AND_the_creation_date_is_set_to_now()
            {
                Assert.IsTrue (_createdDate >= _startTime);
            }
        }

        public class WHEN_the_creation_model_is_null : ReleseCreationService.ICreateReleases
        {
            public void Create (ReleseCreationService.ReleaseToCreate model)
            {
                throw new NotImplementedException ();
            }

            [Test]
            public void THEN_the_release_is_not_created()
            {
                bool? releaseCreated = null;
                new ReleseCreationService(this, () => releaseCreated = false, () => releaseCreated = true).Create(null);
                Assert.False (releaseCreated.Value);
            }
        }
    }

    public class ReleseCreationService
    {
        readonly ICreateReleases _repository;
        readonly Action _onReleaseNotCreated;
        readonly Action _onReleaseCreated;

        public class ReleaseToCreate
        {
            public DateTime Created{ get; set; }
        }

        public interface ICreateReleases
        {
            void Create (ReleaseToCreate model);
        }

        public ReleseCreationService (ICreateReleases repository, Action onReleaseNotCreated, Action onReleaseCreated)
        {
            _repository = repository;
            _onReleaseCreated = onReleaseCreated;
            _onReleaseNotCreated = onReleaseNotCreated;
        }

        public void Create (ReleaseToCreate release)
        {
            if (release == null)
                _onReleaseNotCreated ();
            else {
                release.Created = DateTime.Now;
                _onReleaseCreated ();
                _repository.Create (release);
            }
        }
    }

}

