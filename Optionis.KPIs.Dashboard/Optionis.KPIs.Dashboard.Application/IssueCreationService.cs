using System;
using System.ComponentModel;
using Optionis.KPIs.Dashboard.Application.Validators;

namespace Optionis.KPIs.Dashboard.Application
{
    public class IssueCreationService
    {
        readonly Action<int> _onIssueCreated;
        readonly Action<ValidationError> _onValidationError;
        readonly ValidateObject<Issue, ValidationError> _validators;
        readonly ICreateIssues _repository;

        public interface ICreateIssues
        {
            void Create(Issue issue, Action<int> onIssueCreated);
        }

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            EmptyIssueId = 1,
            EmptyTitle = 2,
            InvalidIssueId = 3,
            InvalidTitle = 4,
            InvalidLink = 5
        }

        public class Issue
        {
            public string IssueId { get; set; }
            public string Link { get; set; }
            public int ReleaseId { get; set; }
            public string Title { get; set; }
        }

        public IssueCreationService(Action<ValidationError> onValidationError, Action<int> onIssueCreated, ICreateIssues repository)
        {
            _onIssueCreated = onIssueCreated;
            _onValidationError = onValidationError;
            _repository = repository;
            _validators =
                new ValidateObject<Issue, ValidationError>(
                    _onValidationError,
                    CreateIssue,
                    new ValidateIssueIdIsNotEmpty(),
                    new ValidateTitleNotEmpty(),
                    new ValidateLengthLessThan<Issue, ValidationError>(25, () => ValidationError.InvalidIssueId, issue => issue.IssueId),
                    new ValidateLengthLessThan<Issue, ValidationError>(255, () => ValidationError.InvalidTitle, issue => issue.Title),
                    new ValidateLengthLessThan<Issue, ValidationError>(255, () => ValidationError.InvalidLink, issue => issue.Link));
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
                _repository.Create(issue, _onIssueCreated);
        }

        class ValidateIssueIdIsNotEmpty : IValidateObjects<Issue, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.EmptyIssueId; } }

            public bool IsValid(Issue issue)
            {
                return !string.IsNullOrEmpty(issue.IssueId);
            }
        }

        class ValidateTitleNotEmpty : IValidateObjects<Issue, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.EmptyTitle; } }

            public bool IsValid(Issue issue)
            {
                return !string.IsNullOrEmpty(issue.Title);
            }
        }
    }
}