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
                new ReleseCreationService().Create(() => releaseCreated = true);
                Assert.True (releaseCreated);
            }
        }
    }

    public class ReleseCreationService
    {
        public void Create (Action onReleaseCreated)
        {
            onReleaseCreated ();
        }
    }

}

