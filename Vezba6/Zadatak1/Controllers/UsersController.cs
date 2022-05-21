using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak1.Models;

namespace Zadatak1.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Delete(string username)
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];
            users.Remove(username);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Search(string role)
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];
            List<User> foundUsers = new List<User>();

            if (role.Equals(""))
            {
                ViewBag.users = users.Values;
            }
            else
            {
                foreach (User user in users.Values)
                {
                    if (user.Role == role)
                    {
                        foundUsers.Add(user);
                    }
                }
                ViewBag.users = foundUsers;
            }

            return View("~/Views/Home/Index.cshtml");
        }
    }
}