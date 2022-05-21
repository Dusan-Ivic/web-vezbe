using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak1.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        
        public int Age { get; set; }
        public bool LoggedIn { get; set; }

        public User()
        {
            Username = "";
            Password = "";
            LoggedIn = false;
        }

        public User(string username, string password, string role, int age)
        {
            Username = username;
            Password = password;
            Role = role;
            Age = age;
            LoggedIn = false;
        }
    }
}