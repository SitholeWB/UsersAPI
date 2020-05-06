using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Commands
{
	public class SetUserRoleCommand
	{
		[Required]
		public string Role { get; set; }
	}
}
