using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Models.BookViewModels
{
    public class BookDetailsViewModel
    {
        [HiddenInput]
        public int Id {get;set;}

        [Editable(false)]
        public string AddedBy {get;set;}

        [Editable(false)]
        public string Title {get;set;}

        [Editable(false)]
        public string Author {get;set;}

        [Editable(false)]
        public string Publisher {get;set;}

        [Editable(false)]
        public DateTime RelaseDate {get;set;}

        [Editable(false)]
        public string ISBN {get;set;}
        
        [Editable(false)]
        public string Language {get;set;}
    }
}