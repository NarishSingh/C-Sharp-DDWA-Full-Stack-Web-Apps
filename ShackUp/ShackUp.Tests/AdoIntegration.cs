using System.Collections.Generic;
using NUnit.Framework;
using ShackUp.Data.ADO;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;

namespace ShackUp.Tests
{
    [TestFixture]
    public class AdoIntegration
    {
        [Test]
        public void LoadAllStates()
        {
            IStatesRepo repo = new StatesRepoADO();
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
        public void LoadAllBathroomTypes()
        {
            IBathroomTypesRepo repo = new BathroomTypesRepoADO();
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