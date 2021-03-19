namespace ShackUp.Models.Db
{
    public class Listing
    {
        public int ListingId { get; set; }
        public string UserId { get; set; }
        public string StateId { get; set; }
        public int BathroomTypeId { get; set; }
        public string Nickname { get; set; }
        public string City { get; set; }
        public decimal Rate { get; set; }
        public decimal SquareFootage { get; set; }
        public bool HasElectric { get; set; }
        public bool HasHeat { get; set; }
        public string ListingDescription { get; set; }
        public string ImageFileName { get; set; }
        //omitted prop's are for db use only
    }
}