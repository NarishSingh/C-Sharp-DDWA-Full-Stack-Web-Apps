using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;

namespace ShackUp.Data.ADO
{
    public class StatesRepoADO : IStatesRepo
    {
        public List<State> ReadAllStates()
        {
            List<State> states = new List<State>();
            
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["ShackUp"]
                    .ConnectionString;

                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "StatesSelectAll",
                    Connection = c,
                    CommandType = CommandType.StoredProcedure
                };

                c.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        State row = new State
                        {
                            StateId = dr["StateId"].ToString(),
                            StateName = dr["StateName"].ToString()
                        };
                        
                        states.Add(row);
                    }
                }
            }

            return states;
        }
    }
}