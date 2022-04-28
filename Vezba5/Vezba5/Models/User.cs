using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vezba5.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }

        public bool LoggedIn { get; set; }

        public User()
        {
            FirstName = "";
            LastName = "";
            Username = "";
            Password = "";
            LoggedIn = false;
        }

        public User(string firstName, string lastName, string username, string password, UserType type)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Type = type;
            LoggedIn = false;
        }

        public void Logoff()
        {
            FirstName = "";
            LastName = "";
            Username = "";
            Password = "";
            LoggedIn = false;
        }

        public bool Login()
        {
            if (Users.users.ContainsKey(Username) && Users.users[Username].Password.Equals(Password))
            {
                LoggedIn = true;
            }

            return LoggedIn;
        }

        public bool Register()
        {
            if (!Users.users.ContainsKey(Username))
            {
                Users.users.Add(Username, this);
                return true;
            }
            return false;
        }

        public void Logout()
        {
            LoggedIn = false;
        }
    }
}