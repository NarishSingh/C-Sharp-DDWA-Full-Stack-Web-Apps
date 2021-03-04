using System;

namespace MovieCatalogEntity.Models.EF
{
    public class Movie
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        public int? RatingId { get; set; }
        public string Title { get; set; }
        //new additions after setting up migration
        public DateTime? ReleaseDate { get; set; }
        public int? DirectorId { get; set; }
        
        //Navigator properties are tagged with virtual -> will handle table joins
        public virtual Genre Genre { get; set; }
        public virtual Rating Rating { get; set; }
        public virtual Director Director { get; set; }
    }
}