using Models.Commands.Responses;
using Models.Entities;

namespace Services.Helpers
{
	public static class RolesModelsHelper
	{
		public static RoleResponse ConvertRoleToRoleResponse(Role roleEntity)
		{
			if (roleEntity == null)
			{
				return null;
			}
			return new RoleResponse
			{
				Description = roleEntity.Description,
				DateAdded = roleEntity.DateAdded,
				Id = roleEntity.Id,
				LastModifiedDate = roleEntity.LastModifiedDate,
				Name = roleEntity.Name
			};
		}
	}
}