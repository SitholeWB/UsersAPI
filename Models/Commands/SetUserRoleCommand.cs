using System.ComponentModel.DataAnnotations;

namespace Models.Commands
{
	public class SetUserRoleCommand
	{
		[Required]
		public string Role { get; set; }
	}
}