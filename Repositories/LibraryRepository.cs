using System;
using System.Linq;
using HomeLibrary.Models;

namespace HomeLibrary.Repositories
{
    public class LibraryReposiotry : ILibraryRepository
    {
        private readonly ApplicationDbContext _context;

        public LibraryReposiotry(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddLibrary(Library library)
        {
            _context.Libraries.Add(library);
        }

        public Library GetUserLibrary(string userId)
        {
            return _context.Libraries.Where(x=>x.ApplicationUserId == userId).FirstOrDefault();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}