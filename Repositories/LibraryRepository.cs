using System;
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

        public void AddInvitation(Invitation invitation)
        {
            _context.Invitations.Add(invitation);
        }

        public void AddLibrary(Library library)
        {
            _context.Libraries.Add(library);
        }

        public void AddUserToLibrary(int libraryId, string email)
        {
            var library = _context.Libraries.Where(x=>x.Id == libraryId).Include(x=>x.Users).FirstOrDefault();

            if(library != null)
            {
                var user = _context.Users.Where(x=>x.Email == email).FirstOrDefault();

                if(user != null)
                {
                    //library.Users.Add(user);
                }
            }
        }

        public Library GetUserLibrary(string userId)
        {
            return _context.Libraries.Where(x=>x.OwnerId == userId)
                .Include(x=>x.Books)
                    .ThenInclude(x=>x.ApplicationUser)
                .Include(x=>x.Users)
                .FirstOrDefault();
        }

        public void RemoveInvitation(int libraryId, string email)
        {
            var invitation = _context.Invitations.Where(x=>x.LibraryId == libraryId && x.Email == email).FirstOrDefault();

            if(invitation != null)
            {
                _context.Invitations.Remove(invitation);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}