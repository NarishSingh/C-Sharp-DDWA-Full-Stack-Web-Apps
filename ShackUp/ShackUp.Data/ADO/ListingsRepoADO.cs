using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;

namespace ShackUp.Data.ADO
{
    public class ListingsRepoADO : IListingRepo
    {
        public void CreateListing(Listing listing)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["ShackUp"]
                    .ConnectionString;

                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingInsert",
                    CommandType = CommandType.StoredProcedure
                };

                //ouput param
                SqlParameter param = new SqlParameter("@ListingId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(param);

                //other params for insert
                cmd.Parameters.AddWithValue("@UserId", listing.UserId);
                cmd.Parameters.AddWithValue("@StateId", listing.StateId);
                cmd.Parameters.AddWithValue("@BathroomTypeId", listing.BathroomTypeId);
                cmd.Parameters.AddWithValue("@Nickname", listing.Nickname);
                cmd.Parameters.AddWithValue("@City", listing.City);
                cmd.Parameters.AddWithValue("@Rate", listing.Rate);
                cmd.Parameters.AddWithValue("@SquareFootage", listing.SquareFootage);
                cmd.Parameters.AddWithValue("@HasElectric", listing.HasElectric);
                cmd.Parameters.AddWithValue("@HasHeat", listing.HasHeat);
                cmd.Parameters.AddWithValue("@ListingDescription", listing.ListingDescription);
                cmd.Parameters.AddWithValue("@ImageFileName", listing.ImageFileName);

                c.Open();

                cmd.ExecuteNonQuery();
                listing.ListingId = (int) param.Value;
            }
        }

        public Listing ReadListingById(int listingId)
        {
            Listing listing = null;

            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["ShackUp"]
                    .ConnectionString;

                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingSelect",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ListingId", listingId);

                c.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        listing = new Listing
                        {
                            ListingId = (int) dr["ListingId"],
                            UserId = dr["UserId"].ToString(),
                            StateId = dr["StateId"].ToString(),
                            BathroomTypeId = (int) dr["BathroomTypeId"],
                            Nickname = dr["Nickname"].ToString(),
                            City = dr["City"].ToString(),
                            Rate = (decimal) dr["Rate"],
                            SquareFootage = (decimal) dr["SquareFootage"],
                            HasElectric = (bool) dr["HasElectric"],
                            HasHeat = (bool) dr["HasHeat"]
                        };

                        if (dr["ListingDescription"] != DBNull.Value)
                        {
                            listing.ListingDescription = dr["ListingDescription"].ToString();
                        }

                        if (dr["ImageFileName"] != DBNull.Value)
                        {
                            listing.ImageFileName = dr["ImageFileName"].ToString();
                        }
                    }
                }
            }

            return listing;
        }

        public void UpdateListing(Listing listing)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteListing(int listingId)
        {
            throw new System.NotImplementedException();
        }
    }
}