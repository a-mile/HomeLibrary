using System.ComponentModel.DataAnnotations;

namespace HomeLibrary.Models.LibraryViewModels
{
    public class InviteUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email {get;set;}
    }
}