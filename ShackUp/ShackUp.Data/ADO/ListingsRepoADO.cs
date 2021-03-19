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
            throw new System.NotImplementedException();
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