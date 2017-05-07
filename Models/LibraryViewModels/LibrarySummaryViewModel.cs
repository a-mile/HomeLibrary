namespace HomeLibrary.Models.LibraryViewModels
{
    public class LibrarySummaryViewModel
    {
        public int Id {get;set;}
        public string Owner {get;set;}
        public int BooksCount {get;set;}
        public int UsersCount {get;set;}
    }
}