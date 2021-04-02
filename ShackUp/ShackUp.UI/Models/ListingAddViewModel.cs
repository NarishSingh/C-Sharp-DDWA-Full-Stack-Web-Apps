using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShackUp.Models.Db;

namespace ShackUp.UI.Models
{
    public class ListingAddViewModel : IValidatableObject
    {
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> BathroomTypes { get; set; }
        public Listing Listing { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Listing.Nickname))
            {
                errors.Add(new ValidationResult("City is required"));
            }

            if (string.IsNullOrEmpty(Listing.City))
            {
                errors.Add(new ValidationResult("City is required"));
            }

            if (string.IsNullOrEmpty(Listing.ListingDescription))
            {
                errors.Add(new ValidationResult("Description is required"));
            }

            if (ImageUpload != null && ImageUpload.ContentLength > 0)
            {
                //even if uploaded, verify if file extension is valid for persistence 
                string[] extensions = {".jpg", ".png", ".gif", ".jpeg"};

                string uploadExtension = Path.GetExtension(ImageUpload.FileName);

                if (!extensions.Contains(uploadExtension))
                {
                    errors.Add(new ValidationResult("Image file must be jpg, png, gif, or jpeg"));
                }
            }
            else
            {
                errors.Add(new ValidationResult("Image required"));
            }

            if (Listing.Rate <= 0)
            {
                errors.Add(new ValidationResult("Rate must be > $0"));
            }

            if (Listing.SquareFootage <= 0)
            {
                errors.Add(new ValidationResult("Square footage must be < 0 sq. ft."));
            }

            return errors;
        }
    }
}