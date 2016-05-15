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
        readonly Action _onReleaseCreated;

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
            InvalidComments = 7
        }

        public class ReleaseToCreate
        {
            public DateTime Created{ get; set; }
            public int CreatedBy{ get; set; }
            public string Version{ get; set; }
            public string Title{ get; set; }
            public string Application{ get; set; }
            public Issue[] Issues{ get; set; }
            public string Comments{ get; set; }
        }

        public class Issue
        {
            public string Id{ get; set; }
            public string Link{ get; set; }
            public string Title{ get; set; }
        }

        public interface ICreateReleases
        {
            void Create (ReleaseToCreate model);
        }

        public interface ICheckUsersExist
        {
            void UserExists (Action onUserNotExist, Action onUserExist);
        }

        public interface IValidateReleasesToCreate
        {
            bool IsValid(ReleaseToCreate release);
        }

        public ReleseCreationService (
            ICreateReleases repository,
            ICheckUsersExist userRepository,
            Action<ValidationError> onValidationError,
            Action onReleaseCreated)
        {
            _onReleaseCreated = onReleaseCreated;
            _onValidationError = onValidationError;
            _repository = repository;
            _userRepository = userRepository;
        }

        public void Create (ReleaseToCreate release)
        {
            ModelIsValid (release, _onValidationError, CreateRelease);
        }

        void CreateRelease(ReleaseToCreate release)
        {
            _repository.Create (release);
            _onReleaseCreated ();
        }

        void ModelIsValid(ReleaseToCreate release, Action<ValidationError> onInvalid, Action<ReleaseToCreate> onValid)
        {
            foreach (var validationMethod in Validators) {
                if (!validationMethod.Value.IsValid (release)) {
                    onInvalid (validationMethod.Key);
                    return;
                }
            }
            onValid (release);
        }

        IDictionary<ValidationError, ReleseCreationService.IValidateReleasesToCreate> Validators
        {
            get{
                return new Dictionary<ValidationError, ReleseCreationService.IValidateReleasesToCreate> {
                    { ValidationError.ObjectNotSet, new ValidateReleaseIsSet() },
                    { ValidationError.TitleNotSet, new ValidateTitle () },
                    { ValidationError.ApplicationNotSet, new ValidateApplication() },
                    { ValidationError.InvalidVersion, new ValidateVersion() },
                    { ValidationError.UserNotFound, new ValidateCreationUser (_userRepository) },
                    { ValidationError.InvalidIssue, new ValidateIssues () },
                    { ValidationError.InvalidComments, new ValidateComments() }
                };
            }
        }
    }

    class ValidateReleaseIsSet : ReleseCreationService.IValidateReleasesToCreate
    {
        public bool IsValid(ReleseCreationService.ReleaseToCreate release)
        {
            return release != null;
        }
    }

    class ValidateApplication : ReleseCreationService.IValidateReleasesToCreate
    {
        public bool IsValid (ReleseCreationService.ReleaseToCreate release)
        {
            return !string.IsNullOrEmpty (release.Application);
        }
    }

    class ValidateTitle : ReleseCreationService.IValidateReleasesToCreate
    {
        public bool IsValid (ReleseCreationService.ReleaseToCreate release)
        {
            return !string.IsNullOrEmpty (release.Title);
        }
    }

    class ValidateVersion : ReleseCreationService.IValidateReleasesToCreate
    {
        const string VERSION_REGEX = @"^\d+[.]\d+[.]\d+[.](\d+|\*)$";

        public bool IsValid(ReleseCreationService.ReleaseToCreate release)
        {
            return new Regex (VERSION_REGEX).IsMatch (release.Version);
        }
    }

    class ValidateCreationUser : ReleseCreationService.IValidateReleasesToCreate
    {
        readonly ReleseCreationService.ICheckUsersExist _userRepository;

        public ValidateCreationUser (ReleseCreationService.ICheckUsersExist userRepository)
        {
            _userRepository = userRepository;    
        }

        public bool IsValid (ReleseCreationService.ReleaseToCreate release)
        {
            var userExists = false;
            _userRepository.UserExists (() => userExists = false, () => userExists = true);
            return userExists;
        }
    }

    class ValidateIssues : ReleseCreationService.IValidateReleasesToCreate
    {
        public bool IsValid (ReleseCreationService.ReleaseToCreate release)
        {
            return release.Issues == null ||
                release.Issues.All (issue => !string.IsNullOrEmpty(issue.Id));
        }
    }

    class ValidateComments : ReleseCreationService.IValidateReleasesToCreate
    {
        public bool IsValid (ReleseCreationService.ReleaseToCreate release)
        {
            return string.IsNullOrEmpty(release.Comments) || release.Comments.Length <= 255;
        }
    }
}

