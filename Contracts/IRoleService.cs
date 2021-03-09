using Models.Commands;
using Models.Commands.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IRoleService
	{
		Task<RoleResponse> AddRoleAsync(AddRoleCommand command);

		Task<RoleResponse> UpdateRoleAsync(Guid id, UpdateRoleCommand command);

		Task DeleteRoleByIdAsync(Guid id);

		Task<RoleResponse> GetRoleByIdAsync(Guid id);

		Task<IEnumerable<RoleResponse>> GetRolesAsync();
	}
}