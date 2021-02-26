using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ADO.NET_Queries.Models;

namespace ADO.NET_Queries.Dao
{
    public class SelectQuery
    {
        public List<EmployeePayRate> GetEmployeeRates()
        {
            List<EmployeePayRate> rates = new List<EmployeePayRate>();

            //assign connection
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["SWCCorp"].ToString();

                //command obj
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText =
                        "SELECT e.EmpId, e.FirstName, e.LastName, pr.HourlyRate, pr.MonthlySalary, pr.YearlySalary " +
                        "FROM Employee e " +
                        "JOIN PayRates pr ON e.EmpId = pr.EmpId;"
                };

                conn.Open(); //open connection
                //Execute and read
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    //loop thru query rs
                    while (dr.Read())
                    {
                        //create Model
                        EmployeePayRate row = new EmployeePayRate();

                        //access as if they are KV pairs
                        row.FirstName = dr["FirstName"].ToString();
                        row.LastName = dr["LastName"].ToString();

                        //handle nullables with if statements
                        if (dr["HourlyRate"] != DBNull.Value)
                        {
                            row.HourlyRate = (decimal) dr["HourlyRate"];
                        }

                        if (dr["MonthlySalary"] != DBNull.Value)
                        {
                            row.MonthlySalary = (decimal) dr["MonthlySalary"];
                        }

                        if (dr["YearlySalary"] != DBNull.Value)
                        {
                            row.YearlySalary = (decimal) dr["YearlySalary"];
                        }

                        //pk will never be null
                        row.EmpId = (int) dr["EmpId"];

                        //add to collection
                        rates.Add(row);
                    }
                }
            }

            return rates;
        }
    }
}