using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using ShackUp.Data.ADO;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;
using ShackUp.Models.Queried;

namespace ShackUp.Tests
{
    [TestFixture]
    public class AdoIntegrationTests
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
            Assert.AreEqual("placeholder.jpg", l.ImageFileName);
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
            int testId = testListing.ListingId;
            Listing loaded = repo.ReadListingById(testId);
            Assert.IsNotNull(loaded);

            repo.DeleteListing(testId);
            loaded = repo.ReadListingById(testId);

            Assert.IsNull(loaded);
        }

        [Test]
        public void ReadRecentListings()
        {
            IListingRepo repo = new ListingsRepoADO();

            List<ListingShortItem> listings = repo.ReadAllRecent().ToList();

            Assert.AreEqual(5, listings.Count);
            Assert.AreEqual(6, listings[0].ListingId); //in SP its loaded from CreatedDate descending, so last first
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", listings[0].UserId);
            Assert.AreEqual(150M, listings[0].Rate);
            Assert.AreEqual("Cleveland", listings[0].City);
            Assert.AreEqual("OH", listings[0].StateId);
            Assert.AreEqual("placeholder.jpg", listings[0].ImageFileName);
        }

        [Test]
        public void ReadListingDetails()
        {
            IListingRepo repo = new ListingsRepoADO();
            ListingItem listing = repo.ReadDetailedListingById(1);

            Assert.IsNotNull(listing);

            Assert.AreEqual(1, listing.ListingId);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", listing.UserId);
            Assert.AreEqual("OH", listing.StateId);
            Assert.AreEqual(3, listing.BathroomTypeId);
            Assert.AreEqual("Test shack 1", listing.Nickname);
            Assert.AreEqual("Cleveland", listing.City);
            Assert.AreEqual(100M, listing.Rate);
            Assert.AreEqual(400M, listing.SquareFootage);
            Assert.AreEqual(false, listing.HasElectric);
            Assert.AreEqual(true, listing.HasHeat);
            Assert.AreEqual("placeholder.jpg", listing.ImageFileName);
            Assert.AreEqual("None", listing.BathroomTypeName);
            Assert.AreEqual("Description", listing.ListingDescription);
        }

        [Test]
        public void ReadFavorites()
        {
            IAccountRepo repo = new AccountRepoADO();

            var favorites = repo.ReadFavorites("11111111-1111-1111-1111-111111111111").ToList();

            Assert.AreEqual(2, favorites.Count);

            Assert.AreEqual(1, favorites[0].ListingId);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", favorites[0].UserId);
            Assert.AreEqual("OH", favorites[0].StateId);
            Assert.AreEqual("Cleveland", favorites[0].City);
            Assert.AreEqual(100M, favorites[0].Rate);
            Assert.AreEqual(400M, favorites[0].SquareFootage);
            Assert.AreEqual(false, favorites[0].HasElectric);
            Assert.AreEqual(true, favorites[0].HasHeat);
            Assert.AreEqual("None", favorites[0].BathroomTypeName);
            Assert.AreEqual(3, favorites[0].BathroomTypeId);
        }

        [Test]
        public void ReadContacts()
        {
            IAccountRepo repo = new AccountRepoADO();
            List<ContactRequestItem> contacts = repo.ReadContacts("00000000-0000-0000-0000-000000000000").ToList();

            Assert.AreEqual(2, contacts.Count);

            Assert.AreEqual("11111111-1111-1111-1111-111111111111", contacts[0].UserId);
            Assert.AreEqual("OH", contacts[0].StateId);
            Assert.AreEqual("Cleveland", contacts[0].City);
            Assert.AreEqual(100M, contacts[0].Rate);
            Assert.AreEqual("test2@test.com", contacts[0].Email);
            Assert.AreEqual("Test shack 1", contacts[0].Nickname);
        }

        [Test]
        public void ReadListingsForUser()
        {
            IAccountRepo repo = new AccountRepoADO();
            List<ListingItem> listings = repo.ReadListings("00000000-0000-0000-0000-000000000000").ToList();

            Assert.AreEqual(6, listings.Count);

            Assert.AreEqual(1, listings[0].ListingId);
            Assert.AreEqual("OH", listings[0].StateId);
            Assert.AreEqual(3, listings[0].BathroomTypeId);
            Assert.AreEqual("Test shack 1", listings[0].Nickname);
            Assert.AreEqual("Cleveland", listings[0].City);
            Assert.AreEqual(100M, listings[0].Rate);
            Assert.AreEqual(400M, listings[0].SquareFootage);
            Assert.AreEqual(false, listings[0].HasElectric);
            Assert.AreEqual(true, listings[0].HasHeat);
            Assert.AreEqual("placeholder.jpg", listings[0].ImageFileName);
            Assert.AreEqual("None", listings[0].BathroomTypeName);
        }

        [Test]
        public void CreateDeleteFavorites()
        {
            const string user = "11111111-1111-1111-1111-111111111111";
            IAccountRepo repo = new AccountRepoADO();

            repo.CreateFavorite(user, 3);
            List<FavoriteItem> faves = repo.ReadFavorites(user).ToList();

            Assert.AreEqual(3, faves.Count);

            repo.DeleteFavorite(user, 2);
            faves = repo.ReadFavorites(user).ToList();

            Assert.AreEqual(2, faves.Count);
        }

        [Test]
        public void CreateDeleteContacts()
        {
            const string contactId = "11111111-1111-1111-1111-111111111111";
            const string user = "00000000-0000-0000-0000-000000000000";
            IAccountRepo repo = new AccountRepoADO();

            repo.CreateContact(contactId, 5);
            List<ContactRequestItem> contacts = repo.ReadContacts(user).ToList();

            Assert.AreEqual(3, contacts.Count);

            repo.DeleteContact(contactId, 3);
            contacts = repo.ReadContacts(user).ToList();

            Assert.AreEqual(2, contacts.Count);
        }

        [Test]
        public void CanDetectContact()
        {
            IAccountRepo repo = new AccountRepoADO();
            string userId = "11111111-1111-1111-1111-111111111111";

            bool found = repo.IsContact(userId, 1);
            Assert.IsTrue(found);

            found = repo.IsContact(userId, 10);
            Assert.IsFalse(found);
        }

        [Test]
        public void CanDetectFavorites()
        {
            IAccountRepo repo = new AccountRepoADO();
            string userId = "11111111-1111-1111-1111-111111111111";

            bool found = repo.IsFavorite(userId, 2);
            Assert.IsTrue(found);

            found = repo.IsFavorite(userId, 10);
            Assert.IsFalse(found);
        }
    }
}