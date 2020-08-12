using System.ComponentModel.DataAnnotations;

namespace Models.Commands
{
	public class ForgettenPasswordCommand
	{
		[Required]
		public string Email { get; set; }
	}
}