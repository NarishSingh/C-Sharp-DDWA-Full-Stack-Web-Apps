using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AdoWithSPs.Model;

namespace AdoWithSPs.Dao
{
    public class SelectQuery
    {
        public List<EmployeePayRate> GetEmployeeRates()
        {
            List<EmployeePayRate> rates = new List<EmployeePayRate>();

            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager.ConnectionStrings["SWCCorp"].ConnectionString;

                //stored proc just need to be called by name
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "EmployeeRatesSelect"
                };

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