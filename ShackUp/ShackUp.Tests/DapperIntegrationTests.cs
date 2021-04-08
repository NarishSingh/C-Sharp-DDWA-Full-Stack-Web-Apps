using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using ShackUp.Data;
using ShackUp.Data.Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Db;
using ShackUp.Models.Queried;

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

        [Test]
        public void CanCreateListing()
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
            IListingRepo repo = new ListingsRepoDapper();

            repo.CreateListing(testListing);

            Assert.AreEqual(7, testListing.ListingId);
        }

        [Test]
        public void CanReadListingById()
        {
            IListingRepo repo = new ListingsRepoDapper();
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
        public void ListingNotFoundDapper()
        {
            IListingRepo repo = new ListingsRepoDapper();
            Listing l = repo.ReadListingById(Int32.MaxValue);

            Assert.IsNull(l);
        }

        [Test]
        public void CanReadRecentListings()
        {
            IListingRepo repo = new ListingsRepoDapper();

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
        public void CanReadListingDetails()
        {
            IListingRepo repo = new ListingsRepoDapper();
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
        public void CanUpdateListing()
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
            IListingRepo repo = new ListingsRepoDapper();

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
        public void CanDeleteListing()
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
            IListingRepo repo = new ListingsRepoDapper();

            repo.CreateListing(testListing);
            int testId = testListing.ListingId;
            Listing loaded = repo.ReadListingById(testId);
            Assert.IsNotNull(loaded);

            repo.DeleteListing(testId);
            loaded = repo.ReadListingById(testId);

            Assert.IsNull(loaded);
        }

        [Test]
        public void CanSearchOnMinRate()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> found = repo.Search(new ListingSearchParameters {MinRate = 110M});

            Assert.AreEqual(5, found.Count());
        }

        [Test]
        public void CanSearchOnMaxRate()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> found = repo.Search(new ListingSearchParameters {MaxRate = 110M});

            Assert.AreEqual(2, found.Count());
        }

        [Test]
        public void CanSearchInRangeOfRate()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> found = repo.Search(
                new ListingSearchParameters
                {
                    MinRate = 100M,
                    MaxRate = 120M
                }
            );

            Assert.AreEqual(3, found.Count());
        }

        [Test]
        public void SearchRateFail()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> found = repo.Search(
                new ListingSearchParameters
                {
                    MinRate = Decimal.MaxValue
                }
            );

            Assert.AreEqual(0, found.Count());
        }

        [Test]
        public void CanSearchOnCity()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> found = repo.Search(new ListingSearchParameters {City = "Col"});

            Assert.AreEqual(1, found.Count());

            found = repo.Search(new ListingSearchParameters {City = "Cle"});

            Assert.AreEqual(5, found.Count());
        }

        [Test]
        public void SearchCityFail()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> foundNone = repo.Search(new ListingSearchParameters {City = "The Moon"});
            
            Assert.AreEqual(0, foundNone.Count());
        }

        [Test]
        public void CanSearchStateId()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> found = repo.Search(new ListingSearchParameters {StateId = "OH"});
            
            Assert.AreEqual(6, found.Count());
        }

        [Test]
        public void SearchFailCity()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> foundNone = repo.Search(new ListingSearchParameters {StateId = "XX"});
            
            Assert.AreEqual(0, foundNone.Count());
        }

        [Test]
        public void CanSearchAllOptionalParams()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> found = repo.Search(
                new ListingSearchParameters
                {
                    MinRate = 100M,
                    MaxRate = 120M,
                    City = "Cleveland",
                    StateId = "OH"
                }
            );

            Assert.AreEqual(3, found.Count());
        }

        [Test]
        public void CanSearchNoOptionalParams()
        {
            IListingRepo repo = new ListingsRepoDapper();

            IEnumerable<ListingShortItem> top12 = repo.Search(new ListingSearchParameters());

            Assert.AreEqual(6, top12.Count());
        }
    }
}