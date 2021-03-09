using Models.Entities;

namespace Models.Commands.Responses
{
	public class RoleResponse : BaseEntity
	{
		public string Name { get; set; }

		public string Description { get; set; }
	}
}