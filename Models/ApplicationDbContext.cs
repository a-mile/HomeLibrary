using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Library> Libraries {get;set;}
        public DbSet<Book> Books {get;set;}
        public DbSet<Invitation> Invitations {get;set;}
        public DbSet<LibraryUser> UserLibraries {get;set;}        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LibraryUser>()
            .HasKey(u => new { u.ApplicationUserId, u.LibraryId });

            builder.Entity<LibraryUser>()
            .HasOne(ul => ul.ApplicationUser)
            .WithMany(u => u.OtherLibraries)
            .HasForeignKey(ul => ul.ApplicationUserId);

            builder.Entity<LibraryUser>()
            .HasOne(ul => ul.Library)
            .WithMany(u => u.Users)
            .HasForeignKey(ul => ul.LibraryId);

            base.OnModelCreating(builder);
        }
    }
}