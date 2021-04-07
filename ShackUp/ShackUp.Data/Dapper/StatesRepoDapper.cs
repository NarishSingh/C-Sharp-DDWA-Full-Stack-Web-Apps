using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;

namespace ShackUp.Data.Dapper
{
    public class StatesRepoDapper : IStatesRepo
    {
        public List<State> ReadAllStates()
        {
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                return c.Query<State>("StatesSelectAll", commandType: CommandType.StoredProcedure)
                    .ToList();
            }
        }
    }
}