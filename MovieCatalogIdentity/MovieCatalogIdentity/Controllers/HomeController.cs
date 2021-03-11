using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MovieCatalogIdentity.Data;
using MovieCatalogIdentity.Models;
using MovieCatalogIdentity.Models.Identity;

namespace MovieCatalogIdentity.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            var repo = new MovieRepo();

            return View(repo.GetAllMovies());
        }

        /*LOGIN*/
        /// <summary>
        /// GET - Load login page - public
        /// </summary>
        /// <returns>ActionResult load view for login page</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginVM());
        }

        /// <summary>
        /// POST - Login with account
        /// </summary>
        /// <param name="model">LoginVM with login info</param>
        /// <param name="returnUrl">string to redirect user on login attempt</param>
        /// <returns>ActionResult reload page on fail, or return user to their incoming page after successful login</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userMgr = HttpContext.GetOwinContext().GetUserManager<UserManager<AppUser>>();
            var authMgr = HttpContext.GetOwinContext().Authentication;

            //attempt to load user with password
            AppUser user = userMgr.Find(model.UserName, model.Password); //TODO FIXME db connection issue here

            //if search fails, user will become null then reload
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login");
                return View(model);
            }
            else
            {
                //if successful, set up token cookie and redirect
                var identity = userMgr.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authMgr.SignIn(new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                }, identity);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }

        /*ADD MOVIE*/
        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult AddMovie()
        {
            return null;
        }

        /*EDIT MOVIE*/
        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult EditMovie(int id)
        {
            return null;
        }

        /*DELETE MOVIE*/
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteMovie(int id)
        {
            var repo = new MovieRepo();
            repo.MovieDelete(id);

            return RedirectToAction("Index");
        }
    }
}