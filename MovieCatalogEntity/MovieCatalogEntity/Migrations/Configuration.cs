
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MovieCatalogEntity.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MovieCatalogEntity.Models.EF.MovieCatalogEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    } 
}