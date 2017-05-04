using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Models
{
    public class Invitation
    {
        public int Id {get; set;}
        public string Email {get;set;}
        public DateTime Date {get;set;}

        public int LibraryId {get;set;}
        public Library Library {get;set;}
    } 
}