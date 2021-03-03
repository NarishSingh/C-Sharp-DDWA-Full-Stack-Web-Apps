namespace MovieCatalogEntity.Models.EF
{
    public class Movie
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        public int? RatingId { get; set; }
        public string Title { get; set; }
        
        //Navigator properties are tagged with virtual -> will handle table joins
        public virtual Genre Genre { get; set; }
        public virtual Rating Rating { get; set; }
    }
}