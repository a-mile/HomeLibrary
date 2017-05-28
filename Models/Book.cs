using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Models
{
    public enum LoanType
    {
        OutsideSystem, InsideSystem
    }
    public class Book
    {
        public int Id {get;set;}

        public string ApplicationUserId {get;set;}
        public ApplicationUser ApplicationUser {get;set;}

        public int LibraryId {get;set;}
        public Library Library {get;set;}

        public bool Loan {get;set;}
        public DateTime LoanDate {get;set;}
        public DateTime ReturnDate {get;set;}
        public LoanType LoanType {get;set;}

        public string LoanForUserId {get;set;}

        [ForeignKey("LoanForUserId")]
        public ApplicationUser LoanForUser {get;set;}

        public string Title {get;set;}
        public string Author {get;set;}
        public string Publisher {get;set;}
        public DateTime RelaseDate {get;set;}
        public string ISBN {get;set;}
        public string Language {get;set;}
    }
}