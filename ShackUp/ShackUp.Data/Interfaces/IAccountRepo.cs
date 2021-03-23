using System.Collections.Generic;
using ShackUp.Models.Db;
using ShackUp.Models.Queried;

namespace ShackUp.Data.Interfaces
{
    public interface IAccountRepo
    {
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
    }
}