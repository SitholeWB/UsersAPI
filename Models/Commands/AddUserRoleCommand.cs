using System;

namespace Models.Commands
{
	public class AddUserRoleCommand
	{
		public Guid UserId { get; set; }

		public Guid RoleId { get; set; }
	}
}