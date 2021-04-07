using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using ShackUp.Data;
using ShackUp.Data.Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;

namespace ShackUp.Tests
{
    [TestFixture]
    public class DapperIntegrationTests
    {   
        [SetUp]
        public void Init()
        {   
            //db reset to known state
            using (SqlConnection c = new SqlConnection(Settings.GetConnString()))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = c,
                    CommandText = "DbReset",
                    CommandType = CommandType.StoredProcedure
                };
                
                c.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [Test]
        public void CanLoadAllStates()
        {
            IStatesRepo repo = new StatesRepoDapper();
            List<State> states = repo.ReadAllStates();
            
            Assert.AreEqual(3, states.Count);
            Assert.AreEqual("KY", states[0].StateId);
            Assert.AreEqual("Kentucky", states[0].StateName);
            Assert.AreEqual("MN", states[1].StateId);
            Assert.AreEqual("Minnesota", states[1].StateName);
            Assert.AreEqual("OH", states[2].StateId);
            Assert.AreEqual("Ohio", states[2].StateName);
        }

        [Test]
        public void CanLoadAllBathroomTypes()
        {
            IBathroomTypesRepo repo = new BathroomTypesRepoDapper();
            List<BathroomType> bRooms = repo.ReadAllBathroomTypes();

            Assert.AreEqual(3, bRooms.Count);
            Assert.AreEqual(1, bRooms[0].BathroomTypeId);
            Assert.AreEqual("Indoor", bRooms[0].BathroomTypeName);
            Assert.AreEqual(2, bRooms[1].BathroomTypeId);
            Assert.AreEqual("Outdoor", bRooms[1].BathroomTypeName);
            Assert.AreEqual(3, bRooms[2].BathroomTypeId);
            Assert.AreEqual("None", bRooms[2].BathroomTypeName);
        }
    }
}