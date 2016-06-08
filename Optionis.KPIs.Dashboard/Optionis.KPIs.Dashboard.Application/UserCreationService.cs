using System;

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

        public enum ValidationError
        {
            UserIsNull,
            UserNameEmpty
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
            else
                _repository.Create (user, _onUserCreated);
        }
    }
}
