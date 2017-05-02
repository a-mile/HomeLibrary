using HomeLibrary.Models;

namespace HomeLibrary.Repositories
{
    public interface ILibraryRepository
    {
        void AddLibrary(Library library);
        Library GetUserLibrary(string userId);
        void SaveChanges();
    }
}