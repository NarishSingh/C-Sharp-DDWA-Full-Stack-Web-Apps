using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using ShackUp.Data.ADO;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;

namespace ShackUp.Tests
{
    [TestFixture]
    public class AdoIntegration
    {
        [SetUp]
        public void Init()
        {
            //reset db to a known state for each test
            using (SqlConnection c = new SqlConnection())
            {
                c.ConnectionString = ConfigurationManager
                    .ConnectionStrings["ShackUp"]
                    .ConnectionString;

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

        [Test]
        public void LoadListingById()
        {
            IListingRepo repo = new ListingsRepoADO();
            Listing l = repo.ReadListingById(1);

            Assert.IsNotNull(l);
            Assert.AreEqual(1, l.ListingId);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", l.UserId);
            Assert.AreEqual("OH", l.StateId);
            Assert.AreEqual(3, l.BathroomTypeId);
            Assert.AreEqual("Test shack 1", l.Nickname);
            Assert.AreEqual("Cleveland", l.City);
            Assert.AreEqual(100, l.Rate);
            Assert.AreEqual(400, l.SquareFootage);
            Assert.AreEqual(false, l.HasElectric);
            Assert.AreEqual(true, l.HasHeat);
            Assert.AreEqual("placeholder.png", l.ImageFileName);
            Assert.AreEqual("Description", l.ListingDescription);
        }

        [Test]
        public void ListingNotFound()
        {
            IListingRepo repo = new ListingsRepoADO();
            Listing l = repo.ReadListingById(Int32.MaxValue);

            Assert.IsNull(l);
        }

        [Test]
        public void CreateListing()
        {
            Listing testListing = new Listing
            {
                UserId = "00000000-0000-0000-0000-000000000000",
                StateId = "OH",
                BathroomTypeId = 1,
                Nickname = "My Test Shack",
                City = "Columbus",
                Rate = 50M,
                SquareFootage = 100M,
                HasElectric = true,
                HasHeat = true,
                ImageFileName = "placeholder.png",
                ListingDescription = "Description"
            };
            IListingRepo repo = new ListingsRepoADO();

            repo.CreateListing(testListing);

            Assert.AreEqual(7, testListing.ListingId);
        }

        [Test]
        public void UpdateListing()
        {
            Listing testListing = new Listing
            {
                UserId = "00000000-0000-0000-0000-000000000000",
                StateId = "OH",
                BathroomTypeId = 1,
                Nickname = "My Test Shack",
                City = "Columbus",
                Rate = 50M,
                SquareFootage = 100M,
                HasElectric = true,
                HasHeat = true,
                ImageFileName = "placeholder.png",
                ListingDescription = "Description"
            };
            IListingRepo repo = new ListingsRepoADO();

            repo.CreateListing(testListing);

            testListing.StateId = "KY";
            testListing.Nickname = "update";
            testListing.BathroomTypeId = 2;
            testListing.City = "Louisville";
            testListing.Rate = 25M;
            testListing.SquareFootage = 75M;
            testListing.HasElectric = false;
            testListing.HasHeat = false;
            testListing.ImageFileName = "updated.png";
            testListing.ListingDescription = "updated description";
            
            repo.UpdateListing(testListing);
            Listing updated = repo.ReadListingById(testListing.ListingId);
            
            Assert.NotNull(updated);
            Assert.AreEqual("KY", updated.StateId);
            Assert.AreEqual("update", updated.Nickname);
            Assert.AreEqual(2, updated.BathroomTypeId);
            Assert.AreEqual("Louisville", updated.City);
            Assert.AreEqual(25M, updated.Rate);
            Assert.AreEqual(75M, updated.SquareFootage);
            Assert.AreEqual(false, updated.HasElectric);
            Assert.AreEqual(false, updated.HasHeat);
            Assert.AreEqual("updated.png", updated.ImageFileName);
            Assert.AreEqual("updated description", updated.ListingDescription);
        }

        [Test]
        public void DeleteListing()
        {
            Listing testListing = new Listing
            {
                UserId = "00000000-0000-0000-0000-000000000000",
                StateId = "OH",
                BathroomTypeId = 1,
                Nickname = "My Test Shack",
                City = "Columbus",
                Rate = 50M,
                SquareFootage = 100M,
                HasElectric = true,
                HasHeat = true,
                ImageFileName = "placeholder.png",
                ListingDescription = "Description"
            };
            IListingRepo repo = new ListingsRepoADO();

            repo.CreateListing(testListing);
            int testID = testListing.ListingId;
            Listing loaded = repo.ReadListingById(testID);
            Assert.IsNotNull(loaded);
            
            repo.DeleteListing(testID);
            loaded = repo.ReadListingById(testID);
            
            Assert.IsNull(loaded);

        }
    }
}