using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queried;

namespace ShackUp.Data.Dapper
{
    public class AccountRepoDapper : IAccountRepo
    {
        public void CreateFavorite(string userId, int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@ListingId", listingId);

                c.Execute("FavoritesInsert", param, commandType: CommandType.StoredProcedure);
            }
        }

        public void CreateContact(string userId, int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@ListingId", listingId);

                c.Execute("ContactsInsert", param, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<FavoriteItem> ReadFavorites(string userId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);

                return c.Query<FavoriteItem>("ListingsSelectFavorites", param,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ContactRequestItem> ReadContacts(string userId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);

                return c.Query<ContactRequestItem>("ListingsSelectContacts", param,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ListingItem> ReadListings(string userId)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFavorite(string userId, int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@ListingId", listingId);

                c.Execute("FavoritesDelete", param, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteContact(string userId, int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@ListingId", listingId);

                c.Execute("ContactsDelete", param, commandType: CommandType.StoredProcedure);
            }
        }

        public bool IsContact(string userId, int listingId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsFavorite(string userId, int listingId)
        {
            throw new System.NotImplementedException();
        }
    }
}