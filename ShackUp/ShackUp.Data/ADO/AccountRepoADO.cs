using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queried;

namespace ShackUp.Data.ADO
{
    public class AccountRepoADO : IAccountRepo
    {
        public void CreateFavorite(string userId, int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "FavoritesInsert",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListingId", listingId);
                
                c.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<FavoriteItem> ReadFavorites(string userId)
        {
            List<FavoriteItem> faves = new List<FavoriteItem>();

            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingsSelectFavorites",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);

                c.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        FavoriteItem row = new FavoriteItem
                        {
                            ListingId = (int) dr["ListingId"],
                            UserId = dr["UserId"].ToString(),
                            StateId = dr["StateId"].ToString(),
                            City = dr["City"].ToString(),
                            Rate = (decimal) dr["Rate"],
                            BathroomTypeId = (int) dr["BathroomTypeId"],
                            BathroomTypeName = dr["BathroomTypeName"].ToString(),
                            SquareFootage = (decimal) dr["SquareFootage"],
                            HasElectric = (bool) dr["HasElectric"],
                            HasHeat = (bool) dr["HasHeat"]
                        };

                        faves.Add(row);
                    }
                }
            }

            return faves;
        }

        public IEnumerable<ContactRequestItem> ReadContacts(string userId)
        {
            List<ContactRequestItem> listingContacts = new List<ContactRequestItem>();

            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingsSelectContacts",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);

                c.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ContactRequestItem row = new ContactRequestItem
                        {
                            ListingId = (int) dr["ListingId"],
                            UserId = dr["UserId"].ToString(),
                            StateId = dr["StateId"].ToString(),
                            City = dr["City"].ToString(),
                            Rate = (decimal) dr["Rate"],
                            Email = dr["Email"].ToString(),
                            Nickname = dr["Nickname"].ToString(),
                        };

                        listingContacts.Add(row);
                    }
                }
            }

            return listingContacts;
        }

        public IEnumerable<ListingItem> ReadListings(string userId)
        {
            List<ListingItem> listings = new List<ListingItem>();

            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingsSelectByUser",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);

                c.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListingItem row = new ListingItem
                        {
                            ListingId = (int) dr["ListingId"],
                            UserId = dr["UserId"].ToString(),
                            Nickname = dr["Nickname"].ToString(),
                            StateId = dr["StateId"].ToString(),
                            City = dr["City"].ToString(),
                            Rate = (decimal) dr["Rate"],
                            SquareFootage = (decimal) dr["SquareFootage"],
                            HasElectric = (bool) dr["HasElectric"],
                            HasHeat = (bool) dr["HasHeat"],
                            BathroomTypeId = (int) dr["BathroomTypeId"],
                            BathroomTypeName = dr["BathroomTypeName"].ToString()
                        };

                        if (dr["ImageFileName"] != DBNull.Value)
                        {
                            row.ImageFileName = dr["ImageFileName"].ToString();
                        }

                        listings.Add(row);
                    }
                }
            }

            return listings;
        }

        public void DeleteFavorite(string userId, int listingId)
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "FavoritesDelete",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ListingId", listingId);
                
                c.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}