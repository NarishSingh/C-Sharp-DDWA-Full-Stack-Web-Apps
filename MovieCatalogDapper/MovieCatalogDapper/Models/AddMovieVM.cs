﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MovieCatalogDapper.Models
{
    /// <summary>
    /// View Model for adding a new movie
    /// </summary>
    public class AddMovieVM
    {
        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select a genre for movie")]
        public int SelectedGenreId { get; set; }

        public int? SelectedRatingId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }
        public IEnumerable<SelectListItem> Ratings { get; set; }
    }
}