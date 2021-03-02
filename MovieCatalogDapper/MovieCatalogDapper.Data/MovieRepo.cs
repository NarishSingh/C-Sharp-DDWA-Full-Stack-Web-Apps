using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MovieCatalogDapper.Model;

namespace MovieCatalogDapper.Data
{
    public class MovieRepo
    {
        /// <summary>
        /// Use MovieSelectAll SP to read all movies from db
        /// </summary>
        /// <returns>IEnumerable of all movie records in db</returns>
        public IEnumerable<MovieListView> GetAllMovies()
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager.ConnectionStrings["MovieCatalog"].ConnectionString;

                return c.Query<MovieListView>("MovieSelectAll", commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Read a Movie by its id
        /// </summary>
        /// <param name="id">int for a valid movie id</param>
        /// <returns>Movie for valid id, null otherwise</returns>
        public Movie GetMovieById(int id)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;

                //create param obj and add the param
                DynamicParameters param = new DynamicParameters();
                param.Add("@MovieId", id);

                //use FirstOrDefault to get the first obj found -> Query normally returns IEnumerable
                return c.Query<Movie>(
                        "MovieSelectById",
                        param,
                        commandType: CommandType.StoredProcedure
                    )
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Delete a movie record
        /// </summary>
        /// <param name="id">int for a valid Movie id</param>
        public void MovieDelete(int id)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;

                //param obj
                DynamicParameters param = new DynamicParameters();
                param.Add("MovieId", id);

                //no return, just use .Execute
                c.Execute("MovieDelete", param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Create a new Movie entry
        /// </summary>
        /// <param name="m">Movie obj, well formed</param>
        public void MovieInsert(Movie m)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["MovieCatalog"]
                    .ConnectionString;

                //declare the output param, and other params
                DynamicParameters param = new DynamicParameters();

                param.Add("@MovieId",
                    dbType: DbType.Int32,
                    direction: ParameterDirection.Output
                );
                param.Add("@Title", m.Title);
                param.Add("@GenreId", m.GenreId);
                param.Add("@RatingId", m.RatingId);

                //execute
                c.Execute(
                    "Movieinsert",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                //retrieve the output param
                m.MovieId = param.Get<int>("@MovieId");
            }
        }
    }
}