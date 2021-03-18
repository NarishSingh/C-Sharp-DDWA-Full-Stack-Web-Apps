using System;

namespace ShackUp.Models.Queried
{
    /// <summary>
    /// My account - contact requests
    /// </summary>
    public class ContactRequestItem
    {
        public int ListingId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public decimal Rate { get; set; }
    }
}