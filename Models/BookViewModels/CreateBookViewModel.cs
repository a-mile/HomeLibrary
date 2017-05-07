using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Models.BookViewModels
{
    public class CreateBookViewModel
    {
        [HiddenInput]
        public int LibraryId {get;set;}
        
        [Required]
        public string Title {get;set;}

        [Required]
        public string Author {get;set;}

        [Required]
        public string Publisher {get;set;}

        [Required]
        [Display(Name = "Relase date")]
        public DateTime? RelaseDate {get;set;}

        [Required]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "It is not valid ISBN number.")]
        public string ISBN {get;set;}

        [Required]
        public string Language {get;set;}
    }
}