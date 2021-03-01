using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.Pkcs;
using AdoWithSPs.Model;

namespace AdoWithSPs.Dao
{
    public class SelectWithParams
    {
        public List<EmployeePayRate> GetEmployeeRates(string city)
        {
            List<EmployeePayRate> rates = new List<EmployeePayRate>();

            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager.ConnectionStrings["SWCCorp"].ConnectionString;

                //SP's with params are also called by name
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "EmployeeRatesSelectByCity"
                };

                //add the params for command from method params
                cmd.Parameters.AddWithValue("@City", city);

                c.Open();
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

                    return rates;
                }
            }
        }
    }
}