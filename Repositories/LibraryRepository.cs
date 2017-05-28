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

        public void LoanBookOutsideSystem(Book book)
        {
            book.Loan = true;
            book.LoanType = LoanType.OutsideSystem;
            book.LoanDate = DateTime.Now;
        }

        public void LoanBookInsideSystem(Book book, string userId)
        {
            book.Loan = true;
            book.LoanType = LoanType.InsideSystem;
            book.LoanForUserId = userId;
            book.LoanDate = DateTime.Now;
        }

        public Library GetLibraryById(int libraryId)
        {
            return _context.Libraries.Where(x=>x.Id == libraryId).FirstOrDefault();
        }  

        public Library GetLibraryByOwnerId(string userId)
        {
            return _context.Libraries.Where(x=>x.OwnerId == userId)
                .Include(x=>x.Books)
                    .ThenInclude(x=>x.ApplicationUser)
                .Include(x=>x.Users)
                    .ThenInclude(x=>x.ApplicationUser)
                .Include(x=>x.Invitations)
                .FirstOrDefault();
        }

        public IEnumerable<Library> GetOtherUserLibraries(string userId)
        {
            return _context.Libraries.Where(x => _context.UserLibraries.Any(y => y.ApplicationUserId == userId && y.LibraryId == x.Id))
                .Include(x=>x.Owner)
                .Include(x=>x.Books)
                    .ThenInclude(x=>x.ApplicationUser)
                .Include(x=>x.Users)
                    .ThenInclude(x=>x.ApplicationUser);                
        }

        public IEnumerable<Book> GetAllUserBooks(string userId)
        {
            return _context.Books
                    .Include(x=>x.Library)
                        .ThenInclude(x=>x.Users)
                    .Where(x=>x.Library.OwnerId == userId ||
                        x.Library.Users.Where(y=>y.ApplicationUserId == userId).Any());
        }       

        public IEnumerable<Library> GetAllUserLibraries(string userId)
        {
            return _context.Libraries.Where(x => x.OwnerId == userId || 
                _context.UserLibraries.Any(y => y.ApplicationUserId == userId && y.LibraryId == x.Id))
                .Include(x=>x.Owner)
                .Include(x=>x.Books)
                    .ThenInclude(x=>x.ApplicationUser)
                .Include(x=>x.Users)
                    .ThenInclude(x=>x.ApplicationUser);       
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }        
    }
}