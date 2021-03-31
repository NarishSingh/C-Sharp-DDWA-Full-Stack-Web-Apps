using System.Web.Mvc;
using ShackUp.Data.ADO;
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
    }
}