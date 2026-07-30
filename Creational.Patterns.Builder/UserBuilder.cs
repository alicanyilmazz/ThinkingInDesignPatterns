using System;
using System.Collections.Generic;
using System.Text;

namespace Creational.Patterns.Builder
{
    public class UserBuilder
    {
        private User _user = new User();

        public UserBuilder WithName(string name)
        {
            _user.Name = name;

            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _user.Email = email;

            return this;
        }

        public User Build()
        {
            return _user;
        }
    }
}
