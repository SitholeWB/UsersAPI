using Models.Entities;
using System;

namespace Models.Commands.Responses
{
	public class UserRoleResponse : BaseEntity
	{
		public Guid UserId { get; set; }

		public Guid RoleId { get; set; }

		public string RoleName { get; set; }
	}
}