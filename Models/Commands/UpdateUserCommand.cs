using System.ComponentModel.DataAnnotations;

namespace Models.Commands
{
	public class UpdateUserCommand
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Surname { get; set; }

		[Required]
		public string Gender { get; set; }

		public string About { get; set; }
		public string Country { get; set; }
	}
}