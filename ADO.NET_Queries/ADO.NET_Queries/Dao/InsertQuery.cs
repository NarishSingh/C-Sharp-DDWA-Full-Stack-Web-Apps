using System.Configuration;
using System.Data.SqlClient;

namespace ADO.NET_Queries.Dao
{
    public class InsertQuery
    {
        public void InsertGrant(string grantId, string grantName, decimal amount)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager.ConnectionStrings["SWCCorp"].ConnectionString;

                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText =
                        "INSERT INTO [Grant] (GrantId, GrantName, EmpId, Amount) VALUES (@GrantId, @GrantName, null, @Amount)"
                };

                cmd.Parameters.AddWithValue("@GrantId", grantId);
                cmd.Parameters.AddWithValue("@GrantName", grantName);
                cmd.Parameters.AddWithValue("@Amount", amount);

                c.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}