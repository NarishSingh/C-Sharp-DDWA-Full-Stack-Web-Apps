using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MovieCatalogDapper.Model;

namespace MovieCatalogDapper.Data
{
    public class MovieRepo
    {
        /// <summary>
        /// Use
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MovieListView> GetAllMovies()
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager.ConnectionStrings["MovieCatalog"].ConnectionString;

                return c.Query<MovieListView>("MovieSelectAll", commandType: CommandType.StoredProcedure);
            }
        }
    }
}