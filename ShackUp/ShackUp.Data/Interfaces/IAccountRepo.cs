using System.Collections.Generic;
using ShackUp.Models.Queried;

namespace ShackUp.Data.Interfaces
{
    public interface IAccountRepo
    {
        /// <summary>
        /// Create a favorite listing for user
        /// </summary>
        /// <param name="userId">string for a user account id</param>
        /// <param name="listingId">int for a existing listing</param>
        void CreateFavorite(string userId, int listingId);

        /// <summary>
        /// Read all favorited shacks for user
        /// </summary>
        /// <param name="userId">string for a user account id</param>
        /// <returns>IEnumerable of FavoriteItem viewmodels</returns>
        IEnumerable<FavoriteItem> ReadFavorites(string userId);

        /// <summary>
        /// Read all contacts for user
        /// </summary>
        /// <param name="userId">string for a user account id</param>
        /// <returns>IEnumerable of ContactRequestItem viewmodels</returns>
        IEnumerable<ContactRequestItem> ReadContacts(string userId);

        /// <summary>
        /// Read all listings for a user
        /// </summary>
        /// <param name="userId">string for a user account id</param>
        /// <returns>IEnumerable of ListingItem viewmodels</returns>
        IEnumerable<ListingItem> ReadListings(string userId);

        /// <summary>
        /// Delete a favorite listing for user
        /// </summary>
        /// <param name="userId">string for a user account id</param>
        /// <param name="listingId">int for a existing listing</param>
        void DeleteFavorite(string userId, int listingId);
    }
}