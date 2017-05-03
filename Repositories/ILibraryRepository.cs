using HomeLibrary.Models;

namespace HomeLibrary.Repositories
{
    public interface ILibraryRepository
    {
        void AddLibrary(Library library);
        void AddInvitation(Invitation invitation);
        Library GetUserLibrary(string userId);
        void SaveChanges();
    }
}