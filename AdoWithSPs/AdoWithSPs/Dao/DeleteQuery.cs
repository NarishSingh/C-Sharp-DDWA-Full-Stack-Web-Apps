using System.Configuration;
using System.Data.SqlClient;

namespace AdoWithSPs.Dao
{
    public class DeleteQuery
    {
        public void DeleteGrant(string grantId)
        {
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager.ConnectionStrings["SWCCorp"].ConnectionString;

                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "DELETE FROM [Grant] WHERE GrantId = @GrantId"
                };

                cmd.Parameters.AddWithValue("@GrantId", grantId);

                c.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}