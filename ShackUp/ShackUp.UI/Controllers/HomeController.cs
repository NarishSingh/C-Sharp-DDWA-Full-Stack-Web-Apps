using System.Collections.Generic;
using System.Web.Mvc;
using ShackUp.Data.Factories;
using ShackUp.Models.Queried;

namespace ShackUp.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<ListingShortItem> model = ListingRepositoryFactory.GetRepository().ReadAllRecent();
            return View(model);
        }
    }
}