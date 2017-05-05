using System;
using System.Collections.Generic;
using System.Linq;
using HomeLibrary.Models;
using Microsoft.EntityFrameworkCore;

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

        public Library GetLibraryById(int id)
        {
             return _context.Libraries.Where(x=>x.Id == id)
                .Include(x=>x.Books)
                    .ThenInclude(x=>x.ApplicationUser)
                .Include(x=>x.Users)
                .Include(x=>x.Invitations)
                .FirstOrDefault();
        }

        public Library GetLibraryByOwnerId(string userId)
        {
            return _context.Libraries.Where(x=>x.OwnerId == userId)
                .Include(x=>x.Books)
                    .ThenInclude(x=>x.ApplicationUser)
                .Include(x=>x.Users)
                .Include(x=>x.Invitations)
                .FirstOrDefault();
        }

        public IEnumerable<Library> GetOtherUserLibraries(string userId)
        {
            return _context.Libraries.Where(x => _context.UserLibraries.Any(y => y.ApplicationUserId == userId && y.LibraryId == x.Id))
                .Include(x=>x.Owner)
                .Include(x=>x.Books)
                .Include(x=>x.Users)
                .ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}