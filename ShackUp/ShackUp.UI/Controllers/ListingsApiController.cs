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
        
        /*
        [Route("api/listings/search")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Search(decimal? minRate, decimal? maxRate, string city, string stateId)
        {
            IListingRepo repo = ListingRepositoryFactory.GetRepository();

            try
            {
                ListingSearchParameters p = new ListingSearchParameters()
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
        */
    }
}