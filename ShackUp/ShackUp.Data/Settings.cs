using System.Configuration;

namespace ShackUp.Data
{
    public class Settings
    {
        private static string _connString;
        private static string _repoType;

        /// <summary>
        /// Get the connection string from config
        /// </summary>
        /// <returns>string for connection string to db</returns>
        public static string GetConnString()
        {
            if (string.IsNullOrEmpty(_connString))
            {
                _connString = ConfigurationManager.ConnectionStrings["ShackUp"]
                    .ConnectionString;
            }

            return _connString;
        }

        /// <summary>
        /// Get the repository type from config
        /// </summary>
        /// <returns></returns>
        public static string GetRepositoryType()
        {
            if (string.IsNullOrEmpty(_repoType))
            {
                _repoType = ConfigurationManager.AppSettings["RepositoryType"].ToString();
            }

            return _repoType;
        }
    }
}