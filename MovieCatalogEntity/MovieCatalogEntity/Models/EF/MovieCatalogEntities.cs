using System.Data.Entity;

namespace MovieCatalogEntity.Models.EF
{
    public class MovieCatalogEntities : DbContext
    {
        public MovieCatalogEntities() : base("MovieCatalog")
        {
        }

        //register models in DbContext
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}