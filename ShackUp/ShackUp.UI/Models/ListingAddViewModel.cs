using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ShackUp.Models.Db;

namespace ShackUp.UI.Models
{
    public class ListingAddViewModel
    {
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> BathroomTypes { get; set; }
        public Listing Listing { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }
        
        
    }
}