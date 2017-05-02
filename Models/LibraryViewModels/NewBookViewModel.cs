using System;

namespace HomeLibrary.Models.LibraryViewModels
{
    public class NewBookViewModel
    {
        public string Title {get;set;}
        public string Author {get;set;}
        public string Publisher {get;set;}
        public DateTime RelaseDate {get;set;}
        public string ISBN {get;set;}
        public string Language {get;set;}
    }
}