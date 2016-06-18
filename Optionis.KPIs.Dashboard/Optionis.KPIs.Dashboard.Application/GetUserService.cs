using System;

namespace Optionis.KPIs.Dashboard.Application
{
    public class GetUserService
    {
        readonly Action _onUserNotFound;
        readonly Action<User> _onUserFound;
        readonly IGetUsers _repository;

        public interface IGetUsers
        {
            void Get(int userId, Action onUserNotFound, Action<User> onUserFound);
        }

        public class User
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public DateTime Created{ get; set; }
        }

        public GetUserService (IGetUsers repository, Action onUserNotFound, Action<User> onUserFound)
        {
            _repository = repository;
            _onUserNotFound = onUserNotFound;
            _onUserFound = onUserFound;
        }

        public void Get (int userId)
        {
            _repository.Get (userId, _onUserNotFound, _onUserFound);
        }
    }
}

