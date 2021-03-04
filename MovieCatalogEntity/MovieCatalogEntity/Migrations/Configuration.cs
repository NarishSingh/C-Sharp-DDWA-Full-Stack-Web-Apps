using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using MovieCatalogEntity.Models.EF;

namespace MovieCatalogEntity.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MovieCatalogEntity.Models.EF.MovieCatalogEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MovieCatalogEntities context)
        {
            //add sample genres
            context.Genres.AddOrUpdate(
                g => g.GenreType,
                new Genre {GenreType = "Sci-Fi"},
                new Genre {GenreType = "Adventure"},
                new Genre {GenreType = "Mystery"},
                new Genre {GenreType = "Horror"}
            );

            //add sample ratings
            context.Ratings.AddOrUpdate(
                r => r.RatingName,
                new Rating {RatingName = "G"},
                new Rating {RatingName = "PG"},
                new Rating {RatingName = "PG-13"},
                new Rating {RatingName = "R"}
            );

            context.SaveChanges(); //save changes before adding to fk's

            //add movie
            context.Movies.AddOrUpdate(
                m => m.Title,
                new Movie
                {
                    Title = "Star Wars",
                    GenreId = context.Genres
                        .First(g => g.GenreType == "Sci-Fi")
                        .GenreId,
                    RatingId = context.Ratings
                        .First(r => r.RatingName == "PG")
                        .RatingId
                }
            );
        }
    }
}