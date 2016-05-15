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

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            ObjectNotSet = 1,
            InvalidVersion = 2,
            TitleNotSet = 3,
            ApplicationNotSet = 4,
            UserNotFound = 5,
            InvalidIssue = 6
        }

        public class ReleaseToCreate
        {
            public DateTime Created{ get; set; }
            public int CreatedBy{ get; set; }
            public string Version{ get; set; }
            public string Title{ get; set; }
            public string Application{ get; set; }
            public Issue[] Issues{ get; set; }
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

        public ReleseCreationService (
            ICreateReleases repository,
            ICheckUsersExist userRepository,
            Action<ValidationError> onValidationError)
        {
            _onValidationError = onValidationError;
            _repository = repository;
            _userRepository = userRepository;
        }

        public void Create (ReleaseToCreate release)
        {
            if (ModelIsValid (release)) {
                release.Created = DateTime.Now;
                _repository.Create (release);
            }
        }

        bool ModelIsValid(ReleaseToCreate release){
            foreach (var validationMethod in ValidationMethods) {
                if (validationMethod.Value (release)) {
                    _onValidationError (validationMethod.Key);
                    return false;
                }
            }
            return true;
        }

        IDictionary<ValidationError, Func<ReleaseToCreate, bool>> ValidationMethods
        {
            get{
                return new Dictionary<ValidationError, Func<ReleaseToCreate, bool>> {
                    { ValidationError.ObjectNotSet, ReleaseIsNull },
                    { ValidationError.TitleNotSet, TitleNotSet },
                    { ValidationError.ApplicationNotSet, ApplicationNotSet },
                    { ValidationError.InvalidVersion, InvalidVersion },
                    { ValidationError.UserNotFound, CreatedByUserNotFound },
                    { ValidationError.InvalidIssue, IssuesAreNotValid }
                };
            }
        }

        static bool ReleaseIsNull (ReleaseToCreate release)
        {
            return release == null;
        }

        static bool TitleNotSet(ReleaseToCreate release)
        {
            return string.IsNullOrEmpty (release.Title);
        }

        static bool ApplicationNotSet(ReleaseToCreate release)
        {
            return string.IsNullOrEmpty (release.Application);
        }

        static bool InvalidVersion(ReleaseToCreate release)
        {
            const string regex = @"^\d+[.]\d+[.]\d+[.](\d+|\*)$";
            return !new Regex (regex).IsMatch (release.Version);
        }

        bool CreatedByUserNotFound(ReleaseToCreate release)
        {
            var userExists = false;
            _userRepository.UserExists (() => userExists = false, () => userExists = true);
            return !userExists;
        }

        static bool IssuesAreNotValid(ReleaseToCreate release)
        {
            return release.Issues != null && release.Issues.Any (issue => string.IsNullOrEmpty(issue.Id));
        }
    }
}

