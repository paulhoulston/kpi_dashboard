using System;
using System.ComponentModel;
using NUnit.Framework;
using Optionis.KPIs.Dashboard.Application.Validators;

namespace Optionis.KPIs.Dashboard.Application.Tests
{
    public class GIVEN_I_want_to_create_an_issue
    {
        public class WHEN_I_supply_all_valid_properties
        {
            bool _issueCreated;
            readonly IssueCreationService _service;
            IssueCreationService.ValidationError? _validationError;

            public WHEN_I_supply_all_valid_properties()
            {
                _service = new IssueCreationService(error => _validationError = error, () => _issueCreated = true);
                _service.Create(new IssueCreationService.Issue
                {
                    IssueId = "CR-12345",
                    Title = "Testing issue"
                });
            }

            [Test]
            public void THEN_the_issue_is_created()
            {
                Assert.IsTrue(_issueCreated);
            }

            [Test]
            public void AND_there_is_no_validation_error()
            {
                Assert.IsNull(_validationError);
            }
        }

        public class WHEN_the_issue_ID_is_empty
        {
            bool _issueCreated;
            IssueCreationService.ValidationError? _validationError;
            readonly IssueCreationService _service;

            public WHEN_the_issue_ID_is_empty()
            {
                _service = new IssueCreationService(error => _validationError = error, () => _issueCreated = true);
                _service.Create(new IssueCreationService.Issue
                {
                    Title = "No issue ID on this item"
                });
            }

            [Test]
            public void THEN_the_issue_is_not_created()
            {
                Assert.IsFalse(_issueCreated);
            }

            [Test]
            public void AND_a_issue_empty_validation_message_is_returned()
            {
                Assert.AreEqual(IssueCreationService.ValidationError.EmptyIssueId, _validationError);
            }
        }

        public class WHEN_the_issue_title_is_empty
        {
            bool _issueCreated;
            IssueCreationService.ValidationError? _validationError;
            readonly IssueCreationService _service;

            public WHEN_the_issue_title_is_empty()
            {
                _service = new IssueCreationService(error => _validationError = error, () => _issueCreated = true);
                _service.Create(new IssueCreationService.Issue
                {
                    IssueId = "CR-12345"
                });
            }

            [Test]
            public void THEN_the_issue_is_not_created()
            {
                Assert.IsFalse(_issueCreated);
            }

            [Test]
            public void AND_a_issue_empty_validation_message_is_returned()
            {
                Assert.AreEqual(IssueCreationService.ValidationError.EmptyTitle, _validationError);
            }
        }

        [TestFixture(true, "CR-123456789012345678901")]
        [TestFixture(true, "CR-1234567890123456789012")]
        [TestFixture(false, "CR-12345678901234567890123")]
        public class WHEN_the_issue_id_exceeds_25_characters
        {
            bool _issueCreated;
            IssueCreationService.ValidationError? _validationError;
            readonly IssueCreationService _service;
            readonly bool _isValid;

            public WHEN_the_issue_id_exceeds_25_characters(bool isValid, string issueId)
            {
                _isValid = isValid;
                _service = new IssueCreationService(error => _validationError = error, () => _issueCreated = true);
                _service.Create(new IssueCreationService.Issue
                {
                    IssueId = issueId,
                    Title = "Checking length of issue ID"
                });
            }

            [Test]
            public void THEN_the_issue_is_not_created()
            {
                Assert.AreEqual(_isValid, _issueCreated);
            }

            [Test]
            public void AND_a_issue_too_long_validation_message_is_returned()
            {
                if (!_isValid)
                    Assert.AreEqual(IssueCreationService.ValidationError.InvalidIssueId, _validationError);
            }
        }

        [TestFixture(true, "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123")]
        [TestFixture(true, "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234")]
        [TestFixture(false, "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345")]
        public class WHEN_the_issue_title_exceeds_255_characters
        {
            bool _issueCreated;
            IssueCreationService.ValidationError? _validationError;
            readonly IssueCreationService _service;
            readonly bool _isValid;

            public WHEN_the_issue_title_exceeds_255_characters(bool isValid, string title)
            {
                _isValid = isValid;
                _service = new IssueCreationService(error => _validationError = error, () => _issueCreated = true);
                _service.Create(new IssueCreationService.Issue
                {
                    IssueId = "CR-56734",
                    Title = title
                });
            }

            [Test]
            public void THEN_the_issue_is_not_created()
            {
                Assert.AreEqual(_isValid, _issueCreated);
            }

            [Test]
            public void AND_a_issue_too_long_validation_message_is_returned()
            {
                if (!_isValid)
                    Assert.AreEqual(IssueCreationService.ValidationError.InvalidTitle, _validationError);
            }
        }

        [TestFixture(true, "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123")]
        [TestFixture(true, "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234")]
        [TestFixture(false, "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345")]
        public class WHEN_the_issue_link_exceeds_255_characters
        {
            bool _issueCreated;
            IssueCreationService.ValidationError? _validationError;
            readonly IssueCreationService _service;
            readonly bool _isValid;

            public WHEN_the_issue_link_exceeds_255_characters(bool isValid, string link)
            {
                _isValid = isValid;
                _service = new IssueCreationService(error => _validationError = error, () => _issueCreated = true);
                _service.Create(new IssueCreationService.Issue
                {
                    IssueId = "CR-56734",
                    Title = "Testing hyperlink length",
                    Link = link
                });
            }

            [Test]
            public void THEN_the_issue_is_not_created()
            {
                Assert.AreEqual(_isValid, _issueCreated);
            }

            [Test]
            public void AND_a_issue_too_long_validation_message_is_returned()
            {
                if (!_isValid)
                    Assert.AreEqual(IssueCreationService.ValidationError.InvalidLink, _validationError);
            }
        }
    }
}
