using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Models
{
    public class Loan
    {
        public int Id {get;set;}

        public int BookId {get;set;}

        [ForeignKey("BookId")]
        public Book Book {get;set;}

        public string UserId {get;set;}

        [ForeignKey("UserId")]
        public ApplicationUser User {get;set;}

        public DateTime LoanDate {get;set;}
        public DateTime ReturnDate {get;set;}
    }
}