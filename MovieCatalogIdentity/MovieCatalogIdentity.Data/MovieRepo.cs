using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MovieCatalogIdentity.Models;

namespace MovieCatalogIdentity.Data
{
    public class MovieRepo
    {
        /// <summary>
        /// Create new Movie entry
        /// </summary>
        /// <param name="movie">Well formed Movie obj</param>
        public void MovieInsert(Movie movie)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;

                DynamicParameters param = new DynamicParameters();
                
                //output params
                param.Add("@MovieId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@Title", movie.Title);
                param.Add("@GenreId", movie.GenreId);
                param.Add("@RatingId", movie.RatingId);

                c.Execute("MovieInsert", param, commandType: CommandType.StoredProcedure);
                
                //set the id after successful persistence
                movie.MovieId = param.Get<int>("@MovieId");
            }
        }

        /// <summary>
        /// Read a Movie by its id
        /// </summary>
        /// <param name="id">int for an id</param>
        /// <returns>Movie obj from db corresponding to valid id, null otherwise</returns>
        public Movie GetMovieById(int id)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;

                DynamicParameters param = new DynamicParameters();
                param.Add("@MovieId", id);

                return c.Query<Movie>("MovieSelectById", param, commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();
            }
        }
        
        /// <summary>
        /// Read all Movie entries from db
        /// </summary>
        /// <returns>IEnumerable of MovieListView for all Movies</returns>
        public IEnumerable<MovieListView> GetAllMovies()
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;

                return c.Query<MovieListView>("MovieSelectAll", commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Update a Movie entry
        /// </summary>
        /// <param name="movie">Well formed Movie obj with valid id</param>
        public void MovieEdit(Movie movie)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;
                
                DynamicParameters param = new DynamicParameters();
                
                //output params
                param.Add("@MovieId", movie.MovieId);
                param.Add("@Title", movie.Title);
                param.Add("@GenreId", movie.GenreId);
                param.Add("@RatingId", movie.RatingId);

                c.Execute("MovieUpdate", param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Delete a Movie from db
        /// </summary>
        /// <param name="id">int for an id</param>
        public void MovieDelete(int id)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;
                
                DynamicParameters param = new DynamicParameters();
                param.Add("@MovieId", id);

                c.Execute("MovieDelete", param, commandType: CommandType.StoredProcedure);
            }
        }
        
        /*HELPERS*/
        /// <summary>
        /// Read all Genres from db
        /// </summary>
        /// <returns>IEnumerable of all Genres from db</returns>
        public IEnumerable<Genre> GetGenres()
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;

                return c.Query<Genre>("GenreSelectAll", commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Read all Ratings from db
        /// </summary>
        /// <returns>IEnumerable of all Ratings from db</returns>
        public IEnumerable<Rating> GetRatings()
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;

                return c.Query<Rating>("RatingSelectAll", commandType: CommandType.StoredProcedure);
            }
        }
    }
}