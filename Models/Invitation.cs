using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Models
{
    public class Invitation
    {
        [Key]
        public int Id {get; set;}

        public string Email {get;set;}

        [ForeignKey("Library")]
        public int LibraryId {get;set;}

        public virtual Library Library {get;set;}
    } 
}