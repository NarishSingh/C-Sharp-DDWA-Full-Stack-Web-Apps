using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;
using ShackUp.Models.Queried;

namespace ShackUp.Data.Dapper
{
    public class ListingsRepoDapper : IListingRepo
    {
        public void CreateListing(Listing listing)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();

                //output param
                param.Add(
                    "@ListingId",
                    dbType: DbType.Int32,
                    direction: ParameterDirection.Output
                );

                //other params
                param.Add("@UserId", listing.UserId);
                param.Add("@StateId", listing.StateId);
                param.Add("@BathroomTypeId", listing.BathroomTypeId);
                param.Add("@Nickname", listing.Nickname);
                param.Add("@City", listing.City);
                param.Add("@Rate", listing.Rate);
                param.Add("@SquareFootage", listing.SquareFootage);
                param.Add("@HasElectric", listing.HasElectric);
                param.Add("@HasHeat", listing.HasHeat);
                param.Add("@ImageFileName", listing.ImageFileName);
                param.Add("@ListingDescription", listing.ListingDescription);

                //execute and retrieve id from db
                c.Execute("ListingInsert", param, commandType: CommandType.StoredProcedure);
                listing.ListingId = param.Get<int>("@ListingId");
            }
        }

        public Listing ReadListingById(int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ListingId", listingId);

                return c.Query<Listing>("ListingSelect", param, commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();
            }
        }

        public IEnumerable<ListingShortItem> ReadAllRecent()
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                return c.Query<ListingShortItem>("ListingsSelectRecent", commandType: CommandType.StoredProcedure);
            }
        }

        public ListingItem ReadDetailedListingById(int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ListingId", listingId);

                return c.Query<ListingItem>("ListingsSelectDetails", param, commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();
            }
        }

        public void UpdateListing(Listing listing)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();

                param.Add("@ListingId", listing.ListingId);
                param.Add("@UserId", listing.UserId);
                param.Add("@StateId", listing.StateId);
                param.Add("@BathroomTypeId", listing.BathroomTypeId);
                param.Add("@Nickname", listing.Nickname);
                param.Add("@City", listing.City);
                param.Add("@Rate", listing.Rate);
                param.Add("@SquareFootage", listing.SquareFootage);
                param.Add("@HasElectric", listing.HasElectric);
                param.Add("@HasHeat", listing.HasHeat);
                param.Add("@ImageFileName", listing.ImageFileName);
                param.Add("@ListingDescription", listing.ListingDescription);

                c.Execute("ListingUpdate", param, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteListing(int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ListingId", listingId);

                c.Execute("ListingDelete", param, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ListingShortItem> Search(ListingSearchParameters param)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                DynamicParameters sqlParam = new DynamicParameters();

                string query =
                    "SELECT TOP 12 ListingId, UserId, StateId, City, Rate, ImageFileName FROM Listings WHERE 1 = 1";

                //optional parameters
                if (param.MinRate.HasValue)
                {
                    query += "AND RATE >= @MinRate ";
                    sqlParam.Add("@MinRate", param.MinRate.Value);
                }

                if (param.MaxRate.HasValue)
                {
                    query += "AND RATE <= @MaxRate ";
                    sqlParam.Add("@MaxRate", param.MaxRate.Value);
                }

                //% allows the search function to match similar enough strings, good for search
                if (!string.IsNullOrEmpty(param.City))
                {
                    query += "AND City LIKE @City ";
                    sqlParam.Add("@City", param.City + '%');
                }

                if (!string.IsNullOrEmpty(param.StateId))
                {
                    query += "AND StateId = @StateId ";
                    sqlParam.Add("@StateId", param.StateId);
                }

                query += "ORDER BY CreatedDate DESC";

                return c.Query<ListingShortItem>(query, param, commandType: CommandType.Text);
            }
        }
    }
}