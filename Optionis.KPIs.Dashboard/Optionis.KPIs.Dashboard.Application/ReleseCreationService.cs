using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Optionis.KPIs.Dashboard.Application
{
    public class ReleseCreationService
    {
        readonly ICreateReleases _repository;
        readonly ICheckUsersExist _userRepository;
        readonly Action<ValidationError> _onValidationError;
        readonly Action<int> _onReleaseCreated;
        readonly ValidateObject<ReleaseToCreate, ValidationError> _validators;

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            ObjectNotSet = 1,
            InvalidVersion = 2,
            TitleNotSet = 3,
            ApplicationNotSet = 4,
            UserNotFound = 5,
            InvalidIssue = 6,
            InvalidComments = 7,
            InvalidDeploymentDate = 8
        }

        public class ReleaseToCreate
        {
            public DateTime Created{ get; set; }
            public string CreatedBy{ get; set; }
            public string Version{ get; set; }
            public string Title{ get; set; }
            public string Application{ get; set; }
            public Issue[] Issues{ get; set; }
            public string Comments{ get; set; }
            public DateTime DeploymentDate { get; set; }
        }

        public class Issue
        {
            public string Id{ get; set; }
            public string Link{ get; set; }
            public string Title{ get; set; }
        }

        public interface ICreateReleases
        {
            void Create (ReleaseToCreate model, Action<int> onReleaseCreated);
        }

        public interface ICheckUsersExist
        {
            void UserExists (string userName, Action onUserNotExist, Action onUserExist);
        }

        public ReleseCreationService (
            ICreateReleases repository,
            ICheckUsersExist userRepository,
            Action<ValidationError> onValidationError,
            Action<int> onReleaseCreated)
        {
            _onReleaseCreated = onReleaseCreated;
            _onValidationError = onValidationError;
            _repository = repository;
            _userRepository = userRepository;
            _validators =
                new ValidateObject<ReleaseToCreate, ValidationError> (
                    _onValidationError,
                    CreateRelease,
                    new ValidateReleaseIsSet (),
                    new ValidateTitle (),
                    new ValidateApplication (),
                    new ValidateVersion (),
                    new ValidateCreationUser (_userRepository),
                    new ValidateIssues (),
                    new ValidateComments (),
                    new ValidateDeploymentDate ());
        }

        public void Create (ReleaseToCreate release)
        {
            _validators.IsValid(release);
        }

        void CreateRelease(ReleaseToCreate release)
        {
            _repository.Create (release, _onReleaseCreated);
        }

        class ValidateReleaseIsSet : IValidateObjects<ReleaseToCreate, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.ObjectNotSet; } }

            public bool IsValid(ReleaseToCreate release)
            {
                return release != null;
            }
        }

        class ValidateApplication : IValidateObjects<ReleaseToCreate, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.ApplicationNotSet; } }

            public bool IsValid (ReleaseToCreate release)
            {
                return !string.IsNullOrEmpty (release.Application);
            }
        }

        class ValidateTitle : IValidateObjects<ReleaseToCreate, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.TitleNotSet; } }

            public bool IsValid (ReleaseToCreate release)
            {
                return !string.IsNullOrEmpty (release.Title);
            }
        }

        class ValidateVersion : IValidateObjects<ReleaseToCreate, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.InvalidVersion; } }

            const string VERSION_REGEX = @"^\d+[.]\d+[.]\d+[.](\d+|\*)$";

            public bool IsValid(ReleaseToCreate release)
            {
                return 
                    !string.IsNullOrEmpty(release.Version) &&
                    new Regex (VERSION_REGEX).IsMatch (release.Version);
            }
        }

        class ValidateCreationUser : IValidateObjects<ReleaseToCreate, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.UserNotFound; } }

            readonly ICheckUsersExist _userRepository;

            public ValidateCreationUser (ICheckUsersExist userRepository)
            {
                _userRepository = userRepository;    
            }

            public bool IsValid (ReleaseToCreate release)
            {
                var userExists = false;
                _userRepository.UserExists (release.CreatedBy, () => userExists = false, () => userExists = true);
                return userExists;
            }
        }

        class ValidateIssues : IValidateObjects<ReleaseToCreate, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.InvalidIssue; } }

            public bool IsValid (ReleaseToCreate release)
            {
                return release.Issues == null ||
                    release.Issues.All (issue => !string.IsNullOrEmpty(issue.Id));
            }
        }

        class ValidateComments : IValidateObjects<ReleaseToCreate, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.InvalidComments; } }

            public bool IsValid (ReleaseToCreate release)
            {
                return string.IsNullOrEmpty(release.Comments) || release.Comments.Length <= 255;
            }
        }

        class ValidateDeploymentDate : IValidateObjects<ReleaseToCreate, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.InvalidDeploymentDate; } }

            public bool IsValid (ReleaseToCreate release)
            {
                return release.DeploymentDate > DateTime.Today.AddDays(-30);
            }
        }
    }
}

