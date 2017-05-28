using HomeLibrary.Models;

namespace HomeLibrary.Infrastructure
{
	public interface ICurrentUser
	{
		ApplicationUser User { get; } 
	}
}