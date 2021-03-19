using ShackUp.Models.Db;

namespace ShackUp.Data.Interfaces
{
    public interface IListingRepo
    {
        /// <summary>
        /// Create a new Listing in db
        /// </summary>
        /// <param name="listing">Listing object well formed</param>
        void CreateListing(Listing listing);
        
        /// <summary>
        /// Read a Listing from db by its id
        /// </summary>
        /// <param name="listingId">int for a valid id</param>
        /// <returns>Listing obj from db if successfully read, null otherwise</returns>
        Listing ReadListingById(int listingId);
        
        /// <summary>
        /// Update a Listing in db
        /// </summary>
        /// <param name="listing">Listing obj with updates</param>
        void UpdateListing(Listing listing);
        
        /// <summary>
        /// Delete a Listing from db
        /// </summary>
        /// <param name="listingId">int for a valid id</param>
        void DeleteListing(int listingId);
    }
}