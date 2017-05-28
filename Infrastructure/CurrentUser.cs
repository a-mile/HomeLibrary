using System.Security.Claims;
using System.Security.Principal;
using HomeLibrary.Models;
using Microsoft.AspNetCore.Http;

namespace HomeLibrary.Infrastructure
{
	public class CurrentUser : ICurrentUser
	{		
		private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		private ApplicationUser _user;

		public CurrentUser(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
		{			
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		public ApplicationUser User
		{
			get 
			{
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; 
				return _user ?? (_user = _context.Users.Find(userId)); 
			}
		}
	}
}