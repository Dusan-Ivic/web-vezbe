using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vezba10.Models
{
    public class Users
    {
        public static List<User> UsersList { get; set; } = new List<User>()
        {
            new User() { Id = GenerateId(), FirstName = "Pera", LastName = "Peric"},
            new User() { Id = GenerateId(), FirstName = "Sima", LastName = "Simic"},
            new User() { Id = GenerateId(), FirstName = "Misa", LastName = "Misic"}
        };

        // Create
        public static User AddUser(User user)
        {
            user.Id = GenerateId();
            UsersList.Add(user);
            return user;
        }

        // Read
        public static User FindById(int id)
        {
            return UsersList.Find(item => item.Id == id);
        }

        // Update
        public static User UpdateUser(User user)
        {
            User existingUser = FindById(user.Id);
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            return existingUser;
        }

        // Delete
        public static void RemoveUser(User user)
        {
            UsersList.Remove(user);
        }

        private static int GenerateId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }
    }
}