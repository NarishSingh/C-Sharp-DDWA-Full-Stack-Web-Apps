using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;

namespace ShackUp.Data.ADO
{
    public class BathroomTypesRepoADO : IBathroomTypesRepo
    {
        public List<BathroomType> ReadAllBathroomTypes()
        {
            List<BathroomType> bathroomTypes = new List<BathroomType>();

            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["ShackUp"]
                    .ConnectionString;

                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "BathroomTypesSelectAll",
                    Connection = c,
                    CommandType = CommandType.StoredProcedure
                };

                c.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BathroomType row = new BathroomType
                        {
                            BathroomTypeId = (int) dr["BathroomTypeId"],
                            BathroomTypeName = dr["BathroomTypeName"].ToString()
                        };

                        bathroomTypes.Add(row);
                    }
                }
            }

            return bathroomTypes;
        }
    }
}