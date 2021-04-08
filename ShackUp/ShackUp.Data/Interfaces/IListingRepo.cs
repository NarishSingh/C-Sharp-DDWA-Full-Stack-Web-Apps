using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ShackUp.Models.Db;
using ShackUp.Models.Queried;

namespace ShackUp.Data.Interfaces
{
    public interface IListingRepo
    {
        /// <summary>
        /// Create a new Listing in db
        /// </summary>
        /// <param name="listing">Listing object well formed</param>
        void CreateListing(Listing listing);

        /// <summary>
        /// Read a Listing from db by its id
        /// </summary>
        /// <param name="listingId">int for a valid id</param>
        /// <returns>Listing obj from db if successfully read, null otherwise</returns>
        Listing ReadListingById(int listingId);

        /// <summary>
        /// Read the short models of the 5 most recent Listings
        /// </summary>
        /// <returns>IEnumerable collection of 5 most recent ListingShortItem view models</returns>
        IEnumerable<ListingShortItem> ReadAllRecent();

        /// <summary>
        /// Read the detailed model of a listing
        /// </summary>
        /// <param name="listingId">int for a listing record</param>
        /// <returns>ListingItem detailed viewmodel, null for invalid id</returns>
        ListingItem ReadDetailedListingById(int listingId);

        /// <summary>
        /// Update a Listing in db
        /// </summary>
        /// <param name="listing">Listing obj with updates</param>
        void UpdateListing(Listing listing);

        /// <summary>
        /// Delete a Listing from db
        /// </summary>
        /// <param name="listingId">int for a valid id</param>
        void DeleteListing(int listingId);

        /// <summary>
        /// Search for a listing
        /// </summary>
        /// <param name="searchParams">ListingSearchParameters viewmodel</param>
        /// <returns>IEnumerable of ListingShortItem viewmodels</returns>
        IEnumerable<ListingShortItem> Search(ListingSearchParameters searchParams);
    }
}