using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

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
