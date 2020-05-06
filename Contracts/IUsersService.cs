using Models.Commands;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IUsersService
	{
		Task<User> AddUserAsync(AddUserCommand userCommand);
		Task<User> UpdateUserAsync(User user);
		Task<User> GetUserByUsernameAsync(string username);
		Task<User> GetUserByEmailAsync(string email);
		Task<User> GetUserAsync(Guid id);
		Task<User> SetUserRoleAsync(Guid id, SetUserRoleCommand roleCommand);
		Task<IEnumerable<User>> GetUsersAsync();
	}
}
