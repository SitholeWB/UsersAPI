using Models.Entities;

namespace Models.Commands.Responses
{
	public class UserResponse : BaseEntity
	{
		public string Email { get; set; }
		public string Username { get; set; }

		public string Name { get; set; }
		public string Gender { get; set; }

		public string Surname { get; set; }
		public string AccountAuth { get; set; }

		public string Country { get; set; }

		public string About { get; set; }

		public string Role { get; set; }

		public string Status { get; set; }
	}
}