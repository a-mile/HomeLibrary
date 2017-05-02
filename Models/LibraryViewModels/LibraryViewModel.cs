using System.Collections.Generic;

namespace HomeLibrary.Models.LibraryViewModels
{
    public class LibraryViewModel
    {
        public IEnumerable<BookViewModel> Books {get; set;}
        public IEnumerable<UserViewModel> Users {get;set;}
    }
}