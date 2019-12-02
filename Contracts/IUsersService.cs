using Models.Entities;
using System;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IUsersService
	{
		Task<User> AddUserAsync(User user);
		Task<User> UpdateUserAsync(User user);
		Task<User> GetUserByUsernameAsync(string username);
		Task<User> GetUserByEmailAsync(string email);
		Task<User> GetUserAsync(Guid id);
	}
}
