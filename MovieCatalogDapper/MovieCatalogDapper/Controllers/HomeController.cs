using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MovieCatalogDapper.Data;
using MovieCatalogDapper.Model;
using MovieCatalogDapper.Models;

namespace MovieCatalogDapper.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// GET - Load index page
        /// </summary>
        /// <returns>ActionResult with all movies for display</returns>
        [HttpGet]
        public ActionResult Index()
        {
            MovieRepo repo = new MovieRepo();
            IEnumerable<MovieListView> model = repo.GetAllMovies();

            return View(model);
        }

        /// <summary>
        /// GET - load add movie page
        /// </summary>
        /// <returns>ActionResult with all Genres and Ratings for display</returns>
        [HttpGet]
        public ActionResult AddMovie()
        {
            /*
            MovieRepo repo = new MovieRepo();
            AddMovieVM model = new AddMovieVM
            {
                Genres = from g in repo.GetGenres()
                    select new SelectListItem
                    {
                        Text = g.GenreType,
                        Value = g.GenreId.ToString()
                    },
                Ratings = from r in repo.GetRatings()
                    select new SelectListItem
                    {
                        Text = r.RatingName,
                        Value = r.RatingId.ToString()
                    }
            };
            */

            AddMovieVM model = new AddMovieVM
            {
                Genres = ReadModelGenres(),
                Ratings = ReadModelRatings()
            };

            return View(model);
        }

        /// <summary>
        /// Create a new Movie record
        /// </summary>
        /// <param name="model">AddModelVM with user inputs for persistence</param>
        /// <returns>ActionResult where user is redirected to edit page on successful create, or page is reloaded on fail</returns>
        [HttpPost]
        public ActionResult AddMovie(AddMovieVM model)
        {
            MovieRepo repo = new MovieRepo();

            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    Title = model.Title,
                    RatingId = model.SelectedRatingId,
                    GenreId = model.SelectedGenreId
                };

                repo.MovieInsert(movie);

                return RedirectToAction("EditMovie", new {id = movie.MovieId});
            }
            else
            {
                model.Genres = ReadModelGenres();
                model.Ratings = ReadModelRatings();

                return View(model);
            }
        }

        /// <summary>
        /// GET - load the edit page with the data from the selected Movie
        /// </summary>
        /// <param name="id">int for the Movie's id</param>
        /// <returns>ActionResult where page is loaded for an existing title, or redirected to Index otherwise</returns>
        [HttpGet]
        public ActionResult EditMovie(int id)
        {
            MovieRepo repo = new MovieRepo();
            Movie m = repo.GetMovieById(id);

            //if movie doesn't exist
            if (m == null)
            {
                return RedirectToAction("Index");
            }

            EditMovieVM model = new EditMovieVM
            {
                MovieId = m.MovieId,
                Title = m.Title,
                SelectedGenreId = m.GenreId,
                SelectedRatingId = m.RatingId,
                Genres = ReadModelGenres(),
                Ratings = ReadModelRatings()
            };

            return View(model);
        }

        /// <summary>
        /// POST - Update a movie record
        /// </summary>
        /// <param name="model">EditMovieVM obj with data from user inputs</param>
        /// <returns>ActionResult where user is redirected to Index on successful update, or page reload otherwise</returns>
        [HttpPost]
        public ActionResult EditMovie(EditMovieVM model)
        {
            MovieRepo repo = new MovieRepo();

            if (ModelState.IsValid)
            {
                Movie m = new Movie
                {
                    MovieId = model.MovieId,
                    Title = model.Title,
                    RatingId = model.SelectedRatingId,
                    GenreId = model.SelectedGenreId
                };

                repo.MovieEdit(m);
                return RedirectToAction("Index");
            }
            else
            {
                //fail
                model.Genres = ReadModelGenres();
                model.Ratings = ReadModelRatings();

                return View(model);
            }
        }

        /// <summary>
        /// POST - delete a movie record
        /// </summary>
        /// <param name="id">int for a valid Movie id</param>
        /// <returns>ActionResult where user is redirected to Index on successful delete</returns>
        [HttpPost]
        public ActionResult DeleteMovie(int id)
        {
            MovieRepo repo = new MovieRepo();
            repo.MovieDelete(id);

            return RedirectToAction("Index");
        }

        /*HELPERS*/
        /// <summary>
        /// Populate View Model's Genres field from db
        /// </summary>
        /// <returns>IEnumerable of Genres as SelectListItem</returns>
        private IEnumerable<SelectListItem> ReadModelGenres()
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
        /// Populate View Model's Ratings field from db
        /// </summary>
        /// <returns>IEnumerable of Ratings as SelectListItem</returns>
        private IEnumerable<SelectListItem> ReadModelRatings()
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