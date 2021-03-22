using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queried;

namespace ShackUp.Data.ADO
{
    public class AccountRepoADO : IAccountRepo
    {
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
    }
}