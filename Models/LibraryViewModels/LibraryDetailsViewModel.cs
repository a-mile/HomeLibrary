using System.Collections.Generic;
using HomeLibrary.Models.BookViewModels;

namespace HomeLibrary.Models.LibraryViewModels
{
    public class LibraryDetailsViewModel
    {
        public int Id {get;set;}
        public bool Owned {get;set;}
        public IEnumerable<BookDetailsViewModel> Books {get; set;}
        public IEnumerable<LibraryUserDetailsViewModel> Users {get;set;}
    }
}