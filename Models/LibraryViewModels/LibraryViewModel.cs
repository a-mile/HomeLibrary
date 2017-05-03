using System.Collections.Generic;
using HomeLibrary.Models.BookViewModels;

namespace HomeLibrary.Models.LibraryViewModels
{
    public class LibraryViewModel
    {
        public IEnumerable<ReadBookViewModel> Books {get; set;}
        public IEnumerable<LibraryUserViewModel> Users {get;set;}
    }
}