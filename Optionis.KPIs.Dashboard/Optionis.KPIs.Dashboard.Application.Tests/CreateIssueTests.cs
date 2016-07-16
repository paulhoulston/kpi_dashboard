using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_an_issue
    {
        public class WHEN_I_supply_all_valid_properties
        {
            [Test]
            public void THEN_the_issue_is_created()
            {
                bool issueCreated = false;
                Assert.IsTrue(issueCreated);
            }
        }
    }
}
