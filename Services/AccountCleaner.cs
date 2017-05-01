using System;
using System.Linq;
using HomeLibrary.Models;

namespace HomeLibrary.Services
{
    public class AccountCleaner
    {
        private readonly ApplicationDbContext _context;

        public AccountCleaner(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteUnconfirmedAccounts()
        {
            var unconfirmedAccounts = _context.Users.Where(x=>!x.EmailConfirmed).ToList();

            foreach(var account in unconfirmedAccounts)
            {
                if(DateTime.Now - account.RegisterDate > TimeSpan.FromHours(24))
                    _context.Users.Remove(account);
            }

            _context.SaveChanges();
        }
    }
}