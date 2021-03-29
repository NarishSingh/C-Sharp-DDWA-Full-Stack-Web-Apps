using System;
using System.Web.Http;
using ShackUp.Data.Factories;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Queried;

namespace ShackUp.UI.Controllers
{
    /// <summary>
    /// For the jQuery scripts on Details view
    /// </summary>
    public class ListingsApiController : ApiController
    {
        /*CONTACTS*/
        /// <summary>
        /// Check if a contact exists for user to listing in view
        /// </summary>
        /// <param name="userId">string for the user id of account</param>
        /// <param name="listingId">int for the listing id</param>
        /// <returns>IHttpActionResult 200 OK for successful read, 400 Bad Request if failed</returns>
        [Route("api/contact/check/{userId}/{listingId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult CheckContact(string userId, int listingId)
        {
            IAccountRepo repo = AccountRepositoryFactory.GetRepository();

            try
            {
                bool result = repo.IsContact(userId, listingId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Add a new contact to account
        /// </summary>
        /// <param name="userId">string for the user id of account</param>
        /// <param name="listingId">int for the listing id</param>
        /// <returns>IHttpActionResult 200 OK for successful creation, 400 Bad Request if failed</returns>
        [Route("api/contact/add/{userId}/{listingId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddContact(string userId, int listingId)
        {
            IAccountRepo repo = AccountRepositoryFactory.GetRepository();

            try
            {
                repo.CreateContact(userId, listingId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete a contact from account
        /// </summary>
        /// <param name="userId">string for the user id of account</param>
        /// <param name="listingId">int for the listing id</param>
        /// <returns>IHttpActionResult 200 OK for successful deletion, 400 Bad Request if failed</returns>
        [Route("api/contact/remove/{userId}/{listingId}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult RemoveContact(string userId, int listingId)
        {
            IAccountRepo repo = AccountRepositoryFactory.GetRepository();

            try
            {
                repo.DeleteContact(userId, listingId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /*FAVORITES*/
        [Route("api/favorite/check/{userId}/{listingId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult CheckFavorite(string userId, int listingId)
        {
            IAccountRepo repo = AccountRepositoryFactory.GetRepository();

            try
            {
                bool result = repo.IsFavorite(userId, listingId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Add a favorite listing to account
        /// </summary>
        /// <param name="userId">string for the user id of account</param>
        /// <param name="listingId">int for the listing id</param>
        /// <returns>IHttpActionResult 200 OK for successful creation, 400 Bad Request if failed</returns>
        [Route("api/favorite/add/{userId}/{listingId}")]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddFavorites(string userId, int listingId)
        {
            IAccountRepo repo = AccountRepositoryFactory.GetRepository();

            try
            {
                repo.CreateFavorite(userId, listingId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Remove a favorite listing from account
        /// </summary>
        /// <param name="userId">string for the user id of account</param>
        /// <param name="listingId">int for the listing id</param>
        /// <returns>IHttpActionResult 200 OK for successful deletion, 400 Bad Request if failed</returns>
        [Route("api/favorite/remove/{userId}/{listingId}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult RemoveFavorite(string userId, int listingId)
        {
            IAccountRepo repo = AccountRepositoryFactory.GetRepository();

            try
            {
                repo.DeleteFavorite(userId, listingId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("api/listings/search")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Search(decimal? minRate, decimal? maxRate, string city, string stateId)
        {
            IListingRepo repo = ListingRepositoryFactory.GetRepository();

            try
            {
                ListingSearchParameters p = new ListingSearchParameters
                {
                    MinRate = minRate,
                    MaxRate = maxRate,
                    City = city,
                    StateId = stateId
                };

                return Ok(repo.Search(p));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}