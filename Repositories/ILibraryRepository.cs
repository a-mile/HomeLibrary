using System.Collections.Generic;
using HomeLibrary.Models;

namespace HomeLibrary.Repositories
{
    public interface ILibraryRepository
    {
        void AddLibrary(Library library);
        void LoanBookOutsideSystem(Book book);
        void LoanBookInsideSystem(Book book, string userId);
        Library GetLibraryByOwnerId(string userId);    
        Library GetLibraryById(int libraryId);    
        IEnumerable<Book> GetAllUserBooks(string userId);
        IEnumerable<Library> GetAllUserLibraries(string userId);
        IEnumerable<Library> GetOtherUserLibraries(string userId);
        void SaveChanges();
    }
}