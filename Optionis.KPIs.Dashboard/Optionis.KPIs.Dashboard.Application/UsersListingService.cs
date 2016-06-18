using System.Collections.Generic;
using System;
using Optionis.KPIs.Common;

namespace Optionis.KPIs.Dashboard.Application
{
    public class UserListingService
    {
        readonly IListUsers _userRepository;
        readonly Action<IEnumerable<User>> _onUsersRetrieved;

        public interface IListUsers
        {
            IEnumerable<UserListingService.User> List ();
        }

        public class User
        {
            public User (int userId)
            {
                Uri = Routing.Users.Get(userId);
            }

            public string Uri{ get; private set; }
        }

        public UserListingService (IListUsers userRepository, Action<IEnumerable<User>> onUsersRetrieved)
        {
            _userRepository = userRepository;
            _onUsersRetrieved = onUsersRetrieved;
        }

        public void List()
        {
            _onUsersRetrieved (_userRepository.List ());
        }
    }
}

