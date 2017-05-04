using System;
using System.Linq;
using HomeLibrary.Models;
using HomeLibrary.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HomeLibrary.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public void Initialize()
        {
             _context.Database.EnsureCreated();

             if(_context.Users.Any())
                return;

            var amadi = CreateUser("Amadi", "und3fnd@outlook.com", "Password123!");
            var jan = CreateUser("Jan", "a-mile@outlook.com", "Password123!");
            var karolina = CreateUser("Karolina", "a-mileszko@outlook.com", "Password123!");
             
            var amadiLibrary = new Library()
            {
                OwnerId = amadi.Id
            };

            var janLibrary = new Library()
            {
                OwnerId =jan.Id
            };

            var karolinaLibrary = new Library()
            {
                OwnerId = karolina.Id
            };

            _context.Libraries.Add(amadiLibrary);
            _context.Libraries.Add(janLibrary);
            _context.Libraries.Add(karolinaLibrary);

            _context.SaveChanges();

            _context.Books.Add(new Book(){
                ApplicationUserId = amadi.Id,
                LibraryId = amadiLibrary.Id,
                Title = "Title1",
                Publisher = "Publisher1",
                Author = "Author1",
                Language = "Language1",
                RelaseDate = DateTime.Now,
                ISBN = "ISBN1"
            });

             _context.Books.Add(new Book(){
                ApplicationUserId = jan.Id,
                LibraryId = janLibrary.Id,
                Title = "Title2",
                Publisher = "Publisher2",
                Author = "Author2",
                Language = "Language2",
                RelaseDate = DateTime.Now,
                ISBN = "ISBN2"
            });

             _context.Books.Add(new Book(){
                ApplicationUserId = karolina.Id,
                LibraryId = karolinaLibrary.Id,
                Title = "Title3",
                Publisher = "Publisher3",
                Author = "Author3",
                Language = "Language3",
                RelaseDate = DateTime.Now,
                ISBN = "ISBN3"
            });

            _context.Invitations.Add(new Invitation()
            {
                LibraryId = amadiLibrary.Id,
                Date = DateTime.Now,
                Email = "email@email.com"
            });

            _context.UserLibraries.Add(new UserLibrary()
            {
                LibraryId = amadiLibrary.Id,
                ApplicationUserId = jan.Id
            });

            _context.SaveChanges();
        }

        private ApplicationUser CreateUser(string name, string email, string password)
        {
             var user = new ApplicationUser { UserName = name, Email = email, RegisterDate = DateTime.Now };
             var result = _userManager.CreateAsync(user, password).Result;

             var code = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
             result = _userManager.ConfirmEmailAsync(user, code).Result;

             return user;
        }
    }
}