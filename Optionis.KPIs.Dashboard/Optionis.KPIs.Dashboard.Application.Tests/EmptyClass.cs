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
                bool releaseCreated = false;
                new ReleseCreationService().Create(new ReleseCreationService.ReleaseToCreate(), () => releaseCreated = true);
                Assert.True (releaseCreated);
            }
        }

        public class WHEN_the_creation_model_is_null
        {
            [Test]
            public void THEN_the_release_is_not_created()
            {
                bool releaseCreated = false;
                new ReleseCreationService().Create(null, () => releaseCreated = true);
                Assert.False (releaseCreated);
            }
        }
    }

    public class ReleseCreationService
    {
        public class ReleaseToCreate
        {
        }

        public void Create (ReleaseToCreate release, Action onReleaseCreated)
        {
            if (release != null)
                onReleaseCreated ();
        }
    }

}

