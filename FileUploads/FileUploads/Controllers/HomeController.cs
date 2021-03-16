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
        /// <summary>
        /// GET - Load index page
        /// </summary>
        /// <returns>ActionResult with a ViewModel ready to take a user upload</returns>
        [HttpGet]
        public ActionResult Index()
        {
            FileUploadVM model = new FileUploadVM();
            return View(model);
        }

        /// <summary>
        /// POST - upload a file and store to local "Uploads" directory
        /// </summary>
        /// <param name="model">FileUploadVM with the posted file</param>
        /// <returns>ActionResult reload page on upload</returns>
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

        /// <summary>
        /// Get binary rep of a file for db upload
        /// </summary>
        /// <param name="file">HttpPostedFileBase form user upload</param>
        /// <returns>byte[] to be stored as an "Image" type on MS SQL Server</returns>
        private byte[] ConvertPostedFileToByteArray(HttpPostedFileBase file)
        {
            BinaryReader rdr = new BinaryReader(file.InputStream);
            byte[] imageByte = rdr.ReadBytes((int) file.ContentLength);
            return imageByte;
        }
    }
}