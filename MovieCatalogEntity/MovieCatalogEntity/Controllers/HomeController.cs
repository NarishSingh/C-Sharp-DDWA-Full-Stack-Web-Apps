using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MovieCatalogEntity.Models;
using MovieCatalogEntity.Models.EF;

namespace MovieCatalogEntity.Controllers
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
            MovieCatalogEntities repo = new MovieCatalogEntities();
            var model = repo.Movies
                .Select(m => new MovieListView
                    {
                        MovieId = m.MovieId,
                        Title = m.Title,
                        GenreType = m.Genre.GenreType,
                        RatingName = m.Rating.RatingName
                    }
                );

            return View(model);
        }

        /// <summary>
        /// GET - load add movie page
        /// </summary>
        /// <returns>ActionResult with all Genres and Ratings for display</returns>
        [HttpGet]
        public ActionResult AddMovie()
        {
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
            MovieCatalogEntities repo = new MovieCatalogEntities();

            if (ModelState.IsValid)
            {
                Movie movie = new Movie
                {
                    Title = model.Title,
                    RatingId = model.SelectedRatingId,
                    GenreId = model.SelectedGenreId
                };

                repo.Movies.Add(movie);
                repo.SaveChanges();

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
            MovieCatalogEntities repo = new MovieCatalogEntities();
            Movie movie = repo.Movies
                .FirstOrDefault(m => m.MovieId == id);

            //if movie doesn't exist
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
            MovieCatalogEntities repo = new MovieCatalogEntities();

            if (ModelState.IsValid)
            {
                Movie m = new Movie
                {
                    MovieId = model.MovieId,
                    Title = model.Title,
                    RatingId = model.SelectedRatingId,
                    GenreId = model.SelectedGenreId
                };

                repo.Entry(m).State = EntityState.Modified;
                repo.SaveChanges();

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
            MovieCatalogEntities repo = new MovieCatalogEntities();
            var movie = repo.Movies
                .FirstOrDefault(m => m.MovieId == id);

            if (movie != null)
            {
                repo.Movies.Remove(movie);
                repo.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /*HELPERS*/
        /// <summary>
        /// Populate View Model's Genres field from db
        /// </summary>
        /// <returns>IEnumerable of Genres as SelectListItem</returns>
        private IQueryable<SelectListItem> ReadModelGenres()
        {
            MovieCatalogEntities repo = new MovieCatalogEntities();
            return repo.Genres
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
        private IQueryable<SelectListItem> ReadModelRatings()
        {
            MovieCatalogEntities repo = new MovieCatalogEntities();
            return repo.Ratings
                .Select(r => new SelectListItem
                    {
                        Text = r.RatingName,
                        Value = r.RatingId.ToString()
                    }
                );
        }
    }
}