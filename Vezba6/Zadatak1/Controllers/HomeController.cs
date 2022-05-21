using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak1.Models;

namespace Zadatak1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];
            ViewBag.users = users.Values;

            return View();
        }
    }
}