using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using FileUploads.Models;

namespace FileUploads.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            FileUploadVM model = new FileUploadVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FileUploadVM model)
        {
            //if a file was uploaded
            if (model.Upload != null && model.Upload.ContentLength > 0)
            {
                //convert web path to a dir path and save locally
                string path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(model.Upload.FileName));

                model.Upload.SaveAs(path);
            }

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        /*Get binary rep of a file for db upload*/
        private byte[] ConvertPostedFileToByteArray(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int) file.ContentLength);
            return imageByte;
        }
    }
}