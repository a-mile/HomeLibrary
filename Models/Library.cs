using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Models
{
    public class Library
    {
        public Library()
        {
            Users = new HashSet<ApplicationUser>();
            Books = new HashSet<Book>();
        }

        [Key]
        public int Id {get;set;}

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId {get;set;}

        public virtual ICollection<ApplicationUser> Users {get;set;}
        public virtual ICollection<Book> Books {get;set;}
    }
}