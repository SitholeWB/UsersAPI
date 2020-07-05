using System.ComponentModel.DataAnnotations;

namespace Models.Commands
{
	public class AddUserCommand
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Surname { get; set; }

		[Required]
		public string Gender { get; set; }

		[Required]
		public string Password { get; set; }

		public string AccountAuth { get; set; }
	}
}