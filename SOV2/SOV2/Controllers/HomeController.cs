using SOV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOV2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Avion avion)
        {
            Dictionary<string, Avion> avioni = (Dictionary<string, Avion>)HttpContext.Application["avioni"];

            if (avioni.ContainsKey(avion.Id))
            {
                ViewBag.Message = "Avion sa unetim ID vec postoji!";

                return View("~/Views/Home/Index.cshtml");
            }

            avioni.Add(avion.Id, avion);

            return RedirectToAction("Index", "Airplanes");
        }
    }
}