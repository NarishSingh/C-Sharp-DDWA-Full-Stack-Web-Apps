namespace MovieCatalogDapper.Model
{
    /// <summary>
    /// Will be used for the stored procedure
    /// </summary>
    public class MovieListView
    {
        public int MovieId { get; set; }
        public string GenreType { get; set; }
        public string RatingName { get; set; }
        public string Title { get; set; }
    }
}