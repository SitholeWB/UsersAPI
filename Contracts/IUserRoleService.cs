using Models.Commands;
using Models.Commands.Responses;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IUserRoleService
	{
		Task<UserRoleResponse> AddUserRoleAsync(AddUserRoleCommand command);

		Task DeleteUserRoleByIdAsync(Guid id);

		Task<UserRoleResponse> GetUserRoleByIdAsync(Guid id);

		Task<IEnumerable<UserRoleResponse>> GetUserRolesForUserIdAsync(Guid userId);
	}
}