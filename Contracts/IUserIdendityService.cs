using Models.DTOs;
using Models.Entities;
using System;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IUserIdendityService : IDisposable
	{
		Task<ApplicationUser> GetApplicationUser();

		Task<User> GetAuthorizedUser();
	}
}