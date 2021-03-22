﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;
using ShackUp.Models.Queried;

namespace ShackUp.Data.ADO
{
    public class ListingsRepoADO : IListingRepo
    {
        public void CreateListing(Listing listing)
        {
            using (SqlConnection c = new SqlConnection(GetConnString()))
            {
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
                cmd.Parameters.AddWithValue("@ImageFileName", listing.ImageFileName);
                cmd.Parameters.AddWithValue("@ListingDescription", listing.ListingDescription);

                c.Open();

                cmd.ExecuteNonQuery();
                listing.ListingId = (int) param.Value;
            }
        }

        public Listing ReadListingById(int listingId)
        {
            Listing listing = null;

            using (SqlConnection c = new SqlConnection(GetConnString()))
            {
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

        public IEnumerable<ListingShortItem> ReadAllRecent()
        {
            List<ListingShortItem> listings = new List<ListingShortItem>();

            using (SqlConnection c = new SqlConnection(GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingsSelectRecent",
                    CommandType = CommandType.StoredProcedure
                };

                c.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListingShortItem row = new ListingShortItem
                        {
                            ListingId = (int) dr["ListingId"],
                            UserId = dr["UserId"].ToString(),
                            StateId = dr["StateId"].ToString(),
                            City = dr["City"].ToString(),
                            Rate = (decimal) dr["Rate"]
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

        public ListingItem ReadDetailedListingById(int listingId)
        {
            ListingItem listing = null;

            using (SqlConnection c = new SqlConnection(GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingsSelectDetails",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ListingId", listingId);

                c.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        listing = new ListingItem
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
                            HasHeat = (bool) dr["HasHeat"],
                            BathroomTypeName = dr["BathroomTypeName"].ToString()
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
            using (SqlConnection c = new SqlConnection(GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingUpdate",
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ListingId", listing.ListingId);
                cmd.Parameters.AddWithValue("@UserId", listing.UserId);
                cmd.Parameters.AddWithValue("@StateId", listing.StateId);
                cmd.Parameters.AddWithValue("@BathroomTypeId", listing.BathroomTypeId);
                cmd.Parameters.AddWithValue("@Nickname", listing.Nickname);
                cmd.Parameters.AddWithValue("@City", listing.City);
                cmd.Parameters.AddWithValue("@Rate", listing.Rate);
                cmd.Parameters.AddWithValue("@SquareFootage", listing.SquareFootage);
                cmd.Parameters.AddWithValue("@HasElectric", listing.HasElectric);
                cmd.Parameters.AddWithValue("@HasHeat", listing.HasHeat);
                cmd.Parameters.AddWithValue("@ImageFileName", listing.ImageFileName);
                cmd.Parameters.AddWithValue("@ListingDescription", listing.ListingDescription);

                c.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteListing(int listingId)
        {
            using (SqlConnection c = new SqlConnection(GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "ListingDelete",
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ListingId", listingId);

                c.Open();

                cmd.ExecuteNonQuery();
            }
        }

        /*HELPERS*/
        /// <summary>
        /// Get the connection string from App.config
        /// </summary>
        /// <returns>string for connection string to db</returns>
        private static string GetConnString()
        {
            return ConfigurationManager
                .ConnectionStrings["ShackUp"]
                .ConnectionString;
        }
    }
}