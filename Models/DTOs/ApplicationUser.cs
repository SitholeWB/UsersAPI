using Models.Entities;

namespace Models.DTOs
{
	public class ApplicationUser
	{
		public User AuthenticatedUser { get; set; }
		public User ImpersonatedUser { get; set; }
	}
}