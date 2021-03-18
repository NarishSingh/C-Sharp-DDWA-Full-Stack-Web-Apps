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
        private IStatesRepo _repo;
        
        [SetUp]
        public void Setup()
        {
            _repo = new StatesRepoADO();
        }
        
        [Test]
        public void LoadAllStates()
        {
            List<State> states = _repo.ReadAllStates();

            Assert.AreEqual(3, states.Count);
            Assert.AreEqual("KY", states[0].StateId);
            Assert.AreEqual("Kentucky", states[0].StateName);
            Assert.AreEqual("MN", states[1].StateId);
            Assert.AreEqual("Minnesota", states[1].StateName);
            Assert.AreEqual("OH", states[2].StateId);
            Assert.AreEqual("Ohio", states[2].StateName);
        }
    }
}