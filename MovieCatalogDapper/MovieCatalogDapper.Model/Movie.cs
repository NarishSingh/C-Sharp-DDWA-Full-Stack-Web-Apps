namespace MovieCatalogDapper.Model
{
    public class Movie
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        public int? RatingId { get; set; } //nullable
        public string Title { get; set; }
    }
}