using System.Web.Mvc;
using ShackUp.Data.Factories;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queried;
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
    }
}