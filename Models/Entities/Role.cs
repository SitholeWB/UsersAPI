using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
	[Table("Roles")]
	public class Role : BaseEntity
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public virtual ICollection<UserRole> UserRoles { get; set; }
	}
}