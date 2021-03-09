using Models.Commands.Responses;
using Models.Entities;

namespace Services.Helpers
{
	public static class UserRolesModelsHelper
	{
		public static UserRoleResponse ConvertUserRoleToUserRoleResponse(UserRole userRoleEntity)
		{
			if (userRoleEntity == null)
			{
				return null;
			}
			return new UserRoleResponse
			{
				UserId = userRoleEntity.UserId,
				DateAdded = userRoleEntity.DateAdded,
				Id = userRoleEntity.Id,
				LastModifiedDate = userRoleEntity.LastModifiedDate,
				RoleId = userRoleEntity.RoleId,
				RoleName = userRoleEntity.Role.Name
			};
		}
	}
}