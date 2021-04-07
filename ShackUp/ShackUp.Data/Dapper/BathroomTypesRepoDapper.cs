using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;

namespace ShackUp.Data.Dapper
{
    public class BathroomTypesRepoDapper : IBathroomTypesRepo
    {
        public List<BathroomType> ReadAllBathroomTypes()
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                return c.Query<BathroomType>("BathroomTypesSelectAll", commandType: CommandType.StoredProcedure)
                    .ToList();
            }
        }
    }
}