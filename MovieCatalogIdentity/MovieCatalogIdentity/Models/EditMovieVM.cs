using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MovieCatalogIdentity.Models
{
    public class EditMovieVM
    {
        public int MovieId { get; set; }
        
        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Please select a genre")]
        public int SelectedGenreId { get; set; }

        public int? SelectedRatingId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }
        public IEnumerable<SelectListItem> Ratings { get; set; }
    }
}