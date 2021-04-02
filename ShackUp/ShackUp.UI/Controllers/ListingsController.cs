using System;
using System.IO;
using System.Web.Mvc;
using ShackUp.Data.Factories;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;
using ShackUp.Models.Queried;
using ShackUp.UI.Models;
using ShackUp.UI.Utilities;

namespace ShackUp.UI.Controllers
{
    public class ListingsController : Controller
    {
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.UserId = AuthorizeUtilities.GetUserId(this);
            }

            IListingRepo repo = ListingRepositoryFactory.GetRepository();
            ListingItem model = repo.ReadDetailedListingById(id);

            return View(model);
        }

        [HttpGet]
        public ActionResult Index()
        {
            IStatesRepo repo = StatesRepositoryFactory.GetRepository();

            return View(repo.ReadAllStates());
        }

        [Authorize]
        public ActionResult Add()
        {
            IStatesRepo statesRepo = StatesRepositoryFactory.GetRepository();
            IBathroomTypesRepo broomRepo = BathroomTypesRepositoryFactory.GetRepository();

            ListingAddViewModel model = new ListingAddViewModel
            {
                States = new SelectList(statesRepo.ReadAllStates(), "StateId", "StateId"),
                BathroomTypes = new SelectList(broomRepo.ReadAllBathroomTypes(), "BathroomTypeId",
                    "BathroomTypeName"),
                Listing = new Listing()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(ListingAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                IListingRepo repo = ListingRepositoryFactory.GetRepository();

                try
                {
                    model.Listing.UserId = AuthorizeUtilities.GetUserId(this);

                    if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                    {
                        //build the file path string
                        string savePath = Server.MapPath("~/Images"); //get the local path to images dir

                        string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                        string extension = Path.GetExtension(model.ImageUpload.FileName);

                        string filePath = Path.Combine(savePath, fileName + extension);

                        //need to gurantee that file names are unique so we don't overwrite images in the db
                        int counter = 1;
                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(savePath, fileName + counter.ToString() + extension);
                            counter++;
                        }

                        model.ImageUpload.SaveAs(filePath);
                        model.Listing.ImageFileName = Path.GetFileName(filePath);
                    }

                    repo.CreateListing(model.Listing);

                    return RedirectToAction("Edit", "Listings", new {id = model.Listing.ListingId});
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
                IStatesRepo statesRepo = StatesRepositoryFactory.GetRepository();
                IBathroomTypesRepo broomRepo = BathroomTypesRepositoryFactory.GetRepository();

                model.States = new SelectList(statesRepo.ReadAllStates(), "StateId", "StateId");
                model.BathroomTypes =
                    new SelectList(broomRepo.ReadAllBathroomTypes(), "BathroomTypeId", "BathroomTypeName");

                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit()
        {
            throw new NotImplementedException();
        }
    }
}