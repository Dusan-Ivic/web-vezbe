using SOV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOV2.Controllers
{
    public class AirplanesController : Controller
    {
        // GET: Airplanes
        public ActionResult Index()
        {
            Dictionary<string, Avion> avioni = (Dictionary<string, Avion>)HttpContext.Application["avioni"];
            ViewBag.avioni = avioni.Values;

            return View();
        }

        [HttpPost]
        public ActionResult Search(string proizvodjac)
        {
            Dictionary<string, Avion> avioni = (Dictionary<string, Avion>)HttpContext.Application["avioni"];
            List<Avion> pronadjeniAvioni = new List<Avion>();

            foreach (Avion avion in avioni.Values)
            {
                if (avion.Proizvodjac == proizvodjac)
                {
                    pronadjeniAvioni.Add(avion);
                }
            }

            ViewBag.avioni = pronadjeniAvioni;

            return View("~/Views/Airplanes/Index.cshtml");
        }
    }
}