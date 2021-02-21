using System.ComponentModel.DataAnnotations;

namespace Models.Commands
{
	public class UpdateUserSocialLoginCommand
	{
		public string FacebookJsonData { get; set; }
	}
}