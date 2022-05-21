using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Zadatak2.Models
{
    public class Data
    {
        public static List<Product> ReadProducts(string path)
        {
            List<Product> products = new List<Product>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Product p = new Product(tokens[0], tokens[1], double.Parse(tokens[2]));
                products.Add(p);
            }
            sr.Close();
            stream.Close();

            return products;
        }

        public static List<User> ReadUsers(string path)
        {
            List<User> users = new List<User>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                User p = new User(tokens[0], tokens[1], tokens[2], int.Parse(tokens[3]));
                users.Add(p);
            }
            sr.Close();
            stream.Close();

            return users;
        }

        public static void SaveUser(User user)
        {
            // save user in file users.txt
            FileStream stream = new FileStream("~/App_Data/users.txt", FileMode.Open);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine($"{user.Username};{user.Password};{user.Role};{user.Age}");
        }
    }
}