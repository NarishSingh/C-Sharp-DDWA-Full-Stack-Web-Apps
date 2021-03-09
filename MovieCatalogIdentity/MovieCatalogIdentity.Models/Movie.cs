namespace MovieCatalogIdentity.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        public int? RatingId { get; set; }
        public string Title { get; set; }
    }
}