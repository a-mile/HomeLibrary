using System;
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

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}