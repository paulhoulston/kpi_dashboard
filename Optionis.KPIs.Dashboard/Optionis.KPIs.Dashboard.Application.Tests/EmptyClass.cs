using NUnit.Framework;
using System;

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

            public void Create (ReleseCreationService.ReleaseToCreate model)
            {
                _releaseCreated = true;
                _createdDate = model.Created;
            }

            public WHEN_I_do_supply_a_valid_model ()
            {
                _startTime = DateTime.Now;
                new ReleseCreationService(this).Create(new ReleseCreationService.ReleaseToCreate());
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

            public void Create (ReleseCreationService.ReleaseToCreate model)
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
    }

    public class ReleseCreationService
    {
        readonly ICreateReleases _repository;

        public class ReleaseToCreate
        {
            public DateTime Created{ get; set; }
        }

        public interface ICreateReleases
        {
            void Create (ReleaseToCreate model);
        }

        public ReleseCreationService (ICreateReleases repository)
        {
            _repository = repository;
        }

        public void Create (ReleaseToCreate release)
        {
            if (release == null)
                return;
            
            release.Created = DateTime.Now;
            _repository.Create (release);
        }
    }
}

