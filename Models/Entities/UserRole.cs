using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
	public class UserRole : BaseEntity
	{
		public Guid UserId { get; set; }

		public Guid RoleId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[ForeignKey("RoleId")]
		public virtual Role Role { get; set; }
	}
}