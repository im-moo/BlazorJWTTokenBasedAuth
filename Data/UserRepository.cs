using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorSimpleApplications.Data
{
    public class UserRepository
    {
        public List<User> Users;

        public UserRepository()
        {
            Users = new List<User>()
            {
                new User() {UserName = "A", Password = "A"},
                new User() {UserName = "B", Password = "B"},
            };
        }

        public User GetUser(string username)
        {
            try
            {
                return Users.FirstOrDefault(u => u.UserName == username);
            }
            catch (Exception )
            {
                return null;
            }
        }
    }
}
