using System;
using System.ComponentModel.DataAnnotations;

namespace HomeLibrary.Models.LibraryViewModels
{
    public class NewBookViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Title {get;set;}

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Author {get;set;}

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Publisher {get;set;}

        [Required]
        public DateTime RelaseDate {get;set;}

        [Required]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "It is not valid ISBN number.")]
        public string ISBN {get;set;}

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Language {get;set;}
    }
}