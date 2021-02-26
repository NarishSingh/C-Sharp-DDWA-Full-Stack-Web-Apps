using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ADO.NET_Queries.Models;

namespace ADO.NET_Queries.Dao
{
    public class ParameterizedSelectQuery
    {
        public List<EmployeePayRate> GetEmployeeRates(string city)
        {
            List<EmployeePayRate> rates = new List<EmployeePayRate>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["SWCCorp"].ToString();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "SELECT e.EmpId, e.FirstName, e.LastName, " +
                                  "pr.HourlyRate, pr.MonthlySalary, pr.YearlySalary " +
                                  "FROM Employee e " +
                                  "JOIN PayRates pr ON e.EmpId = pr.EmpId " +
                                  "JOIN [Location] l ON e.LocationId = l.LocationId " +
                                  "WHERE l.City = @city;"
                };

                cmd.Parameters.AddWithValue("@city", city); //inject method params into query

                //loop and read data to model
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        EmployeePayRate row = new EmployeePayRate();

                        row.FirstName = dr["FirstName"].ToString();
                        row.LastName = dr["LastName"].ToString();

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

                        row.EmpId = (int) dr["EmpId"];

                        rates.Add(row);
                    }
                }

                return rates;
            }
        }
    }
}