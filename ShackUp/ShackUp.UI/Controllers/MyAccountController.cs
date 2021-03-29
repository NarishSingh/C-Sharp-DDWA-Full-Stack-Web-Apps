using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ShackUp.UI.Models;
using ShackUp.UI.Utilities;
using System.Web;
using System.Web.Mvc;
using ShackUp.Data.Factories;
using ShackUp.UI.App_Start;

namespace ShackUp.UI.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: MyAccount
        public ActionResult Index()
        {
            var userId = AuthorizeUtilities.GetUserId(this);

            var repo = AccountRepositoryFactory.GetRepository();
            var model = repo.ReadListings(userId);
            return View(model);
        }

        public ActionResult Favorites()
        {
            var userId = AuthorizeUtilities.GetUserId(this);

            var repo = AccountRepositoryFactory.GetRepository();
            var model = repo.ReadFavorites(userId);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteFavorite(int listingId)
        {
            var userId = AuthorizeUtilities.GetUserId(this);

            var repo = AccountRepositoryFactory.GetRepository();
            repo.DeleteFavorite(userId, listingId);

            return RedirectToAction("Favorites");
        }

        public ActionResult Contacts()
        {
            var userId = AuthorizeUtilities.GetUserId(this);

            var repo = AccountRepositoryFactory.GetRepository();
            var model = repo.ReadContacts(userId);
            return View(model);
        }

        public ActionResult UpdateAccount()
        {
            var model = new UpdateAccountViewModel();
            var statesRepo = StatesRepositoryFactory.GetRepository();
            model.States = new SelectList(statesRepo.ReadAllStates(), "StateId", "StateId");
            model.EmailAddress = User.Identity.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateAccount(UpdateAccountViewModel model)
        {
            var currentUser = UserManager.FindByEmail(User.Identity.Name);
            currentUser.UserName = model.EmailAddress;
            currentUser.Email = model.EmailAddress;
            currentUser.StateId = model.StateId;

            UserManager.Update(currentUser);

            return RedirectToAction("UpdateAccount");
        }
    }
}