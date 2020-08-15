using Models.Commands;
using Models.Commands.Responses;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IUsersService
	{
		Task<UserResponse> AddUserAsync(AddUserCommand userCommand);

		Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserCommand command);

		Task<UserResponse> GetUserByUsernameAsync(string username);

		Task<UserResponse> GetUserByEmailAsync(string email);

		Task<UserResponse> GetUserAsync(Guid id);

		Task<UserResponse> SetUserRoleAsync(Guid id, SetUserRoleCommand roleCommand);

		Task<bool> SetUserPasswordAsync(Guid id, SetNewUserPasswordCommand command);

		Task<IEnumerable<UserResponse>> GetUsersAsync();

		Task<User> GetUserEntityByInputAsync(string email = null, string username = null, Guid? id = null);
	}
}