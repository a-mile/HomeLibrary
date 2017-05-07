using System;

namespace HomeLibrary.Models.BookViewModels
{
    public class BookDetailsViewModel
    {
        public int Id {get;set;}
        public string AddedBy {get;set;}
        public string Title {get;set;}
        public string Author {get;set;}
        public string Publisher {get;set;}
        public DateTime RelaseDate {get;set;}
        public string ISBN {get;set;}
        public string Language {get;set;}
    }
}