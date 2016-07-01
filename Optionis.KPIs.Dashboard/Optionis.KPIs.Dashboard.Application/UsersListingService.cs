using System.Collections.Generic;
using System;

namespace Optionis.KPIs.Dashboard.Application
{
    public class UserListingService
    {
        readonly IListUsers _userRepository;
        readonly Action<IEnumerable<User>> _onUsersRetrieved;

        public interface IListUsers
        {
            IEnumerable<User> List ();
        }

        public class User
        {
            public int Id { get; set; }
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

