using System.Configuration;

namespace ShackUp.Data
{
    public class Settings
    {
        /// <summary>
        /// Get the connection string from App.config
        /// </summary>
        /// <returns>string for connection string to db</returns>
        public static string GetConnString()
        {
            return ConfigurationManager
                .ConnectionStrings["ShackUp"]
                .ConnectionString;
        }
    }
}