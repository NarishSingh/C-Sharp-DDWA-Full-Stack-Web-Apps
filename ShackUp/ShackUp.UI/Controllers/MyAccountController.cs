using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ShackUp.UI.Models;
using ShackUp.UI.Utilities;
using System.Web;
using System.Web.Mvc;
using ShackUp.Data.Factories;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queried;
using ShackUp.UI.App_Start;
using ShackUp.UI.Models.Identity;

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
            string userId = AuthorizeUtilities.GetUserId(this);

            IAccountRepo repo = AccountRepositoryFactory.GetRepository();
            IEnumerable<ListingItem> model = repo.ReadListings(userId);
            return View(model);
        }

        public ActionResult Favorites()
        {
            string userId = AuthorizeUtilities.GetUserId(this);

            IAccountRepo repo = AccountRepositoryFactory.GetRepository();
            IEnumerable<FavoriteItem> model = repo.ReadFavorites(userId);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteFavorite(int listingId)
        {
            string userId = AuthorizeUtilities.GetUserId(this);

            IAccountRepo repo = AccountRepositoryFactory.GetRepository();
            repo.DeleteFavorite(userId, listingId);

            return RedirectToAction("Favorites");
        }

        public ActionResult Contacts()
        {
            string userId = AuthorizeUtilities.GetUserId(this);

            IAccountRepo repo = AccountRepositoryFactory.GetRepository();
            IEnumerable<ContactRequestItem> model = repo.ReadContacts(userId);
            return View(model);
        }

        public ActionResult UpdateAccount()
        {
            IStatesRepo statesRepo = StatesRepositoryFactory.GetRepository();
            UpdateAccountViewModel model = new UpdateAccountViewModel
            {
                States = new SelectList(statesRepo.ReadAllStates(), "StateId", "StateId"),
                EmailAddress = User.Identity.Name
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateAccount(UpdateAccountViewModel model)
        {
            AppUser currentUser = UserManager.FindByEmail(User.Identity.Name);
            currentUser.UserName = model.EmailAddress;
            currentUser.Email = model.EmailAddress;
            currentUser.StateId = model.StateId;

            UserManager.Update(currentUser);

            return RedirectToAction("UpdateAccount");
        }
    }
}