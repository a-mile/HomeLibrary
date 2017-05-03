using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Models
{
    public class Book
    {
        [Key]
        public int Id {get;set;}

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId {get;set;}

        [ForeignKey("Library")]
        public int LibraryId {get;set;}

        public string Title {get;set;}
        public string Author {get;set;}
        public string Publisher {get;set;}
        public DateTime RelaseDate {get;set;}
        public string ISBN {get;set;}
        public string Language {get;set;}

        public virtual ApplicationUser ApplicationUser {get;set;}
    }
}