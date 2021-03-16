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
        /// <summary>
        /// GET - load Index page
        /// </summary>
        /// <returns>ActionResult with Index view with all Movies from db</returns>
        [AllowAnonymous]
        [HttpGet]
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
            AppUser user = userMgr.Find(model.UserName, model.Password);

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
        /// <summary>
        /// GET - Load view for adding a new movie entry
        /// </summary>
        /// <returns>ActionResult with a ViewModel for adding new movie</returns>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult AddMovie()
        {
            AddMovieVM model = new AddMovieVM
            {
                Genres = ReadAllGenres(),
                Ratings = ReadAllRatings()
            };

            return View(model);
        }

        /// <summary>
        /// POST - Add a new movie entry to db
        /// </summary>
        /// <param name="model">AddMovieVM with new movie data</param>
        /// <returns>ActionResult to redirect to edit page on successful persistence, reload page otherwise</returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddMovie(AddMovieVM model)
        {
            MovieRepo repo = new MovieRepo();

            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    Title = model.Title,
                    GenreId = model.SelectedGenreId,
                    RatingId = model.SelectedRatingId
                };

                repo.MovieInsert(movie);
                return RedirectToAction("EditMovie", new {id = movie.MovieId});
            }

            //fail
            model.Genres = ReadAllGenres();
            model.Ratings = ReadAllRatings();

            return View(model);
        }

        /*EDIT MOVIE*/
        /// <summary>
        /// GET - Load view for editing a movie entry
        /// </summary>
        /// <param name="id">int for an existing movie entry</param>
        /// <returns>ActionResult load view for an existing movie entry, redirect to Index otherwise</returns>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult EditMovie(int id)
        {
            MovieRepo repo = new MovieRepo();
            var movie = repo.GetMovieById(id);

            if (movie == null)
            {
                return RedirectToAction("Index");
            }

            EditMovieVM model = new EditMovieVM
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                SelectedGenreId = movie.GenreId,
                SelectedRatingId = movie.RatingId,
                Genres = ReadAllGenres(),
                Ratings = ReadAllRatings()
            };

            return View(model);
        }

        /// <summary>
        /// POST - Update a movie record
        /// </summary>
        /// <param name="model">EditMovieVM with updated movie info</param>
        /// <returns>ActionResult redirect to Index on successful update, reload otherwise</returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditMovie(EditMovieVM model)
        {
            MovieRepo repo = new MovieRepo();

            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    MovieId = model.MovieId,
                    Title = model.Title,
                    GenreId = model.SelectedGenreId,
                    RatingId = model.SelectedRatingId
                };

                repo.MovieEdit(movie);

                return RedirectToAction("Index");
            }

            //fail
            model.Genres = ReadAllGenres();
            model.Ratings = ReadAllRatings();

            return View(model);
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

        /*HELPERS*/
        /// <summary>
        /// Read all Genres from db for View
        /// </summary>
        /// <returns>IEnumerable of SelectListItem with Genre type and id</returns>
        private IEnumerable<SelectListItem> ReadAllGenres()
        {
            MovieRepo repo = new MovieRepo();
            return repo.GetGenres()
                .Select(g => new SelectListItem
                    {
                        Text = g.GenreType,
                        Value = g.GenreId.ToString()
                    }
                );
        }

        /// <summary>
        /// Read all Ratings from db for View
        /// </summary>
        /// <returns>IEnumerable of SelectListItem with Rating name and id</returns>
        private IEnumerable<SelectListItem> ReadAllRatings()
        {
            MovieRepo repo = new MovieRepo();
            return repo.GetRatings()
                .Select(r => new SelectListItem
                    {
                        Text = r.RatingName,
                        Value = r.RatingId.ToString()
                    }
                );
        }
    }
}