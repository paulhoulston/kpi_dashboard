using System;
using System.ComponentModel;
using NUnit.Framework;

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
                    IssueId = "CR_12345"
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
                });
            }

            [Test]
            public void THEN_the_issue_is_not_created()
            {
                Assert.IsFalse(_issueCreated);
                Assert.IsNotNull(_validationError);
            }

            [Test]
            public void AND_a_issue_empty_validation_message_is_returned()
            {
                Assert.AreEqual(IssueCreationService.ValidationError.EmptyIssueId, _validationError);
            }
        }
    }

    public class IssueCreationService
    {
        readonly Action _onIssueCreated;
        readonly Action<ValidationError> _onValidationError;
        readonly ValidateObject<Issue, ValidationError> _validators;

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            EmptyIssueId = 1
        }

        public class Issue
        {
            public string IssueId { get; set; }
        }

        public IssueCreationService(Action<ValidationError> onValidationError, Action onIssueCreated)
        {
            _onIssueCreated = onIssueCreated;
            _onValidationError = onValidationError;
                        _validators =
                new ValidateObject<Issue, ValidationError> (
                    _onValidationError,
                    CreateIssue,
                    new ValidateIssueIdIsNotEmpty());
        }

        public void Create(Issue issue)
        {
            _validators.IsValid(issue);
        }

        private void CreateIssue(Issue issue)
        {
            if (string.IsNullOrEmpty(issue.IssueId))
                _onValidationError(ValidationError.EmptyIssueId);
            else
                _onIssueCreated();
        }

        class ValidateIssueIdIsNotEmpty : IValidateObjects<Issue, ValidationError>
        {
            public ValidationError ValidationError { get { return IssueCreationService.ValidationError.EmptyIssueId; } }

            public bool IsValid(Issue issue)
            {
                return !string.IsNullOrEmpty(issue.IssueId);
            }
        }
    }
}
