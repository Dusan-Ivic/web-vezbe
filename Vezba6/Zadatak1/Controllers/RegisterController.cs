using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak1.Models;

namespace Zadatak1.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(User user)
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];
            //bool userExists = users.ContainsKey(user.Username);
            users.Add(user.Username, user);
            return RedirectToAction("Index", "Home");
        }
    }
}