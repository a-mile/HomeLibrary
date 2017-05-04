using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Models
{
    public class Library
    {
        public int Id {get;set;}
        
        public string OwnerId {get;set;}

        [ForeignKey("OwnerId")]
        public ApplicationUser Owner {get;set;}

        public List<UserLibrary> Users {get;set;}
        public List<Book> Books {get;set;}
        public List<Invitation> Invitations {get;set;}
    }
}