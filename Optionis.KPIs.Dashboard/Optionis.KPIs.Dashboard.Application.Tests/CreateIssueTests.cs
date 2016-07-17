using System;
using NUnit.Framework;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_an_issue
    {
        public class WHEN_I_supply_all_valid_properties
        {
            bool _issueCreated;
            readonly IssueCreationService _service;

            public WHEN_I_supply_all_valid_properties()
            {
                _service = new IssueCreationService(() => _issueCreated = true);
                _service.Create();
            }

            [Test]
            public void THEN_the_issue_is_created()
            {
                Assert.IsTrue(_issueCreated);
            }
        }
    }

    public class IssueCreationService
    {
        private readonly Action _onIssueCreated;

        public IssueCreationService(Action onIssueCreated)
        {
            _onIssueCreated = onIssueCreated;
        }

        public void Create()
        {
            _onIssueCreated();
        }
    }
}
