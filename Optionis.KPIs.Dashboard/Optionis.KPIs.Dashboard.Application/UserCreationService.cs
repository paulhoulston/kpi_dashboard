using System;
using System.ComponentModel;

namespace Optionis.KPIs.Dashboard.Application
{
    public class UserCreationService
    {
        readonly Action<ValidationError> _onUserNotCreated;
        readonly Action<int> _onUserCreated;
        readonly ICreateUsers _repository;
        readonly ValidateObject<User, ValidationError> _validators;

        public interface ICreateUsers
        {
            void Create(User user, Action<int> onUserCreated);
        }

        public UserCreationService (ICreateUsers repository, Action<ValidationError> onUserNotCreated, Action<int> onUserCreated)
        {
            _repository = repository;
            _onUserCreated = onUserCreated;
            _onUserNotCreated = onUserNotCreated;
            _validators =
                new ValidateObject<User, ValidationError> (
                    _onUserNotCreated,
                    DoCreateUser,
                    new ValidateUserIsNotNull (),
                    new ValidateUserNameIsNotNull (),
                    new ValidateUserNameIsLessThan50Characters ());
        }

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            UserIsNull = 1,
            UserNameNotSet = 2,
            UserNameTooLong = 3
        }

        public class User
        {
            public string UserName { get; set; }
        }

        public void Create(User user)
        {
            _validators.IsValid (user);
        }

        void DoCreateUser(User user)
        {
            _repository.Create (user, _onUserCreated);
        }

        class ValidateUserIsNotNull : IValidateObjects<User, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.UserIsNull; } }

            public bool IsValid(User user)
            {
                return user != null;
            }
        }

        class ValidateUserNameIsNotNull : IValidateObjects<User, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.UserNameNotSet; } }

            public bool IsValid(User user)
            {
                return !string.IsNullOrEmpty(user.UserName);
            }
        }

        class ValidateUserNameIsLessThan50Characters : IValidateObjects<User, ValidationError>
        {
            public ValidationError ValidationError { get { return ValidationError.UserNameTooLong; } }

            public bool IsValid(User user)
            {
                return user.UserName.Length <= 50;
            }
        }
    }
}