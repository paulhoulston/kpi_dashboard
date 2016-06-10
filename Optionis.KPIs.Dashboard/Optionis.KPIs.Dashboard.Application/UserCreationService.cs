using System;
using System.ComponentModel;

namespace Optionis.KPIs.Dashboard.Application
{
    public class UserCreationService
    {
        readonly Action<ValidationError> _onUserNotCreated;
        readonly Action<int> _onUserCreated;
        readonly ICreateUsers _repository;

        public interface ICreateUsers
        {
            void Create(User user, Action<int> onUserCreated);
        }

        public UserCreationService (ICreateUsers repository, Action<ValidationError> onUserNotCreated, Action<int> onUserCreated)
        {
            _repository = repository;
            _onUserCreated = onUserCreated;
            _onUserNotCreated = onUserNotCreated;
        }

        [DefaultValue(None)]
        public enum ValidationError
        {
            None = 0,
            UserIsNull = 1,
            UserNameEmpty = 2,
            UserNameTooLong = 3
        }

        public class User
        {
            public string UserName { get; set; }
        }

        public void Create(User user)
        {
            if (user == null)
                _onUserNotCreated (ValidationError.UserIsNull);
            else if (string.IsNullOrEmpty (user.UserName))
                _onUserNotCreated (ValidationError.UserNameEmpty);
            else if (user.UserName.Length > 50)
                _onUserNotCreated (ValidationError.UserNameTooLong);
            else
                _repository.Create (user, _onUserCreated);
        }
    }
}
