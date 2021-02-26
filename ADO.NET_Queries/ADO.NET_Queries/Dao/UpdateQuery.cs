using System.Configuration;
using System.Data.SqlClient;

namespace ADO.NET_Queries.Dao
{
    public class UpdateQuery
    {
        public void UpdateGrant(string grantId, string grantName, decimal amount)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager.ConnectionStrings["SWCCorp"].ConnectionString;

                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText =
                        "UPDATE [Grant] SET GrantName = @GrantName, Amount = @Amount WHERE GrantID = @GrantId;"
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