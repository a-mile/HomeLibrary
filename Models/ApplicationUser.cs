using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisterDate {get;set;}

        public Library OwnLibrary {get; set;}
        public List<UserLibrary> Libraries {get;set;}
    }
}