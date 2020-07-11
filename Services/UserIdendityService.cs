using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using Services.DataLayer;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services
{
	public sealed class UserIdendityService : IUserIdendityService
	{
		private readonly IHttpContextAccessor _context;
		private ApplicationUser _applicationUser;
		private readonly UsersDbContext _dbContext;

		public UserIdendityService(IHttpContextAccessor context, UsersDbContext dbContext)
		{
			_context = context;
			_applicationUser = null;
			_dbContext = dbContext;
		}

		public async Task<ApplicationUser> GetApplicationUser()
		{
			if (_applicationUser != null)
			{
				return _applicationUser;
			}
			var userId = _context.HttpContext.User.Claims.First(a => a.Type == ClaimTypes.NameIdentifier);
			var user = await _dbContext.Users.FirstAsync(a => a.Id == Guid.Parse(userId.Value));

			_applicationUser = new ApplicationUser
			{
				AuthenticatedUser = user
			};

			var impersonatedUserId = _context.HttpContext.User.Claims.FirstOrDefault(a => a.Type == "ImpersonatedUserId");
			if (impersonatedUserId != null)
			{
				_applicationUser.ImpersonatedUser = await _dbContext.Users.FirstAsync(a => a.Id == Guid.Parse(impersonatedUserId.Value));
			}
			return _applicationUser;
		}

		public async Task<User> GetAuthorizedUser()
		{
			var appUser = await GetApplicationUser();
			return appUser.ImpersonatedUser ?? appUser.AuthenticatedUser;
		}

		public void Dispose()
		{
			_applicationUser = null;
		}
	}
}