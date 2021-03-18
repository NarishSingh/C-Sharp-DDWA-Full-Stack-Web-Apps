namespace ShackUp.Models.Queried
{
    /// <summary>
    /// For views with small snippets of info - ex. the home page
    /// </summary>
    public class ListingShortItem
    {
        public int ListingId { get; set; }
        public string UserId { get; set; }
        public decimal Rate { get; set; }
        public string StateId { get; set; }
        public string City { get; set; }
        public string ImageFileName { get; set; }
    }
}