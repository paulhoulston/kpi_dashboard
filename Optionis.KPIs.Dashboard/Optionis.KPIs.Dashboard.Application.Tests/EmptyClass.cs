using NUnit.Framework;
using System;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    [TestFixture]
    public class GIVEN_I_want_to_create_a_release
    {
        public class WHEN_I_do_supply_a_valid_model
        {
            [Test]
            public void THEN_the_release_is_created()
            {
                bool? releaseCreated = null;
                new ReleseCreationService(() => releaseCreated = false, () => releaseCreated = true).Create(new ReleseCreationService.ReleaseToCreate());
                Assert.True (releaseCreated.Value);
            }

            [Test]
            public void AND_the_creation_date_is_set_to_now()
            {
                Assert.IsTrue (_createdModel.Created >= DateTime.Now);
            }
        }

        public class WHEN_the_creation_model_is_null
        {
            [Test]
            public void THEN_the_release_is_not_created()
            {
                bool? releaseCreated = null;
                new ReleseCreationService(() => releaseCreated = false, () => releaseCreated = true).Create(null);
                Assert.False (releaseCreated.Value);
            }
        }
    }

    public class ReleseCreationService
    {
        readonly Action _onReleaseNotCreated;
        readonly Action _onReleaseCreated;

        public class ReleaseToCreate
        {
        }

        public ReleseCreationService (Action onReleaseNotCreated, Action onReleaseCreated)
        {
            _onReleaseCreated = onReleaseCreated;
            _onReleaseNotCreated = onReleaseNotCreated;
            
        }

        public void Create (ReleaseToCreate release)
        {
            if (release == null)
                _onReleaseNotCreated ();
            else
                _onReleaseCreated ();
        }
    }

}

