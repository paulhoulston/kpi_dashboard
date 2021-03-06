﻿using System;
using System.ComponentModel;
using System.Linq;
using Optionis.KPIs.Dashboard.Application.Validators;

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
            InvalidComments = 6,
            InvalidDeploymentDate = 7
        }

        public class ReleaseToCreate : IHaveAVersionNumber, IHaveADeploymentDate
        {
            public DateTime Created{ get; set; }
            public string CreatedBy{ get; set; }
            public string Version{ get; set; }
            public string Title{ get; set; }
            public string Application{ get; set; }
            public string Comments{ get; set; }
            public DateTime DeploymentDate { get; set; }
            public DeploymentStatus DeploymentStatus { get; set; }
        }

        public interface ICreateReleases
        {
            void Create (ReleaseToCreate model, Action<int> onReleaseCreated);
        }

        public interface ICheckUsersExist
        {
            void UserExists (string userName, Action onUserNotExist, Action onUserExist);
        }

        public ReleseCreationService(
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
                new ValidateObject<ReleaseToCreate, ValidationError>(
                _onValidationError,
                CreateRelease,
                new ValidateReleaseIsSet(),
                new ValidateTitle(),
                new ValidateApplication(),
                new ValidateVersionNumber<ReleaseToCreate, ValidationError>(() => ValidationError.InvalidVersion),
                new ValidateCreationUser(_userRepository),
                new ValidateLengthLessThan<ReleaseToCreate, ValidationError>(255, () => ValidationError.InvalidComments, release => release.Comments),
                new ValidateDeploymentDate<ReleaseToCreate, ValidationError>(() => ValidationError.InvalidDeploymentDate));
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
    }
}

