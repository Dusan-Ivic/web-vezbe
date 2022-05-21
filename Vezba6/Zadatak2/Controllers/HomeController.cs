using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak2.Models;

namespace Zadatak2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            User user = (User)Session["User"];
            if (user == null || user.Username.Equals(""))
            {
                return RedirectToAction("Index", "Authentication");
            }

            Dictionary<Product, int> cart = (Dictionary<Product, int>)Session["cart"];
            if (cart == null)
            {
                cart = new Dictionary<Product, int>();
                Session["cart"] = cart;
            }

            ViewBag.User = user;
            List<Product> products = (List<Product>)HttpContext.Application["products"];
            
            return View(products);
        }
    }
}