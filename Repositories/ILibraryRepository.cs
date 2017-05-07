using System.Collections.Generic;
using HomeLibrary.Models;

namespace HomeLibrary.Repositories
{
    public interface ILibraryRepository
    {
        void AddLibrary(Library library);
        Library GetLibraryByOwnerId(string userId);
        Library GetLibraryById(int libraryId);
        Book GetBookById(int bookId);
        IEnumerable<Library> GetOtherUserLibraries(string userId);
        void SaveChanges();
    }
}