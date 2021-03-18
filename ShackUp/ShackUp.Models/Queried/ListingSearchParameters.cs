namespace ShackUp.Models.Queried
{
    public class ListingSearchParameters
    {
        public decimal? MinRate { get; set; }
        public decimal? MaxRate { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
    }
}