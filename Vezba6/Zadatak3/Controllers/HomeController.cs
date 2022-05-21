using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak3.Models;

namespace Zadatak3.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<UploadedFile> files = (List<UploadedFile>)HttpContext.Application["files"];
            return View(files);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            List<UploadedFile> files = (List<UploadedFile>)HttpContext.Application["files"];
            try
            {
                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Files/"), fileName);
                    file.SaveAs(path);
                    files.Add(new UploadedFile(fileName, path));
                }
                ViewBag.Message = "File uploaded successfully";
                return RedirectToAction("Index", files);
            }
            catch
            {
                ViewBag.Message = "File upload failed";
                return View("Index", files);
            }
        }
    }
}