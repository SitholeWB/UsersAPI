using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Constants;
using Models.DTOs;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
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

		public async Task<ApplicationUser> GetApplicationUserAsync()
		{
			if (_applicationUser != null)
			{
				return _applicationUser;
			}
			var userId = _context.HttpContext.User.Claims.First(a => a.Type == ClaimTypes.NameIdentifier);
			var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == Guid.Parse(userId.Value));
			if (user == null)
			{
				throw new UserException($"No User found for given token.", ErrorCodes.UserWithGivenIdNotFound);
			}
			_applicationUser = new ApplicationUser
			{
				AuthenticatedUser = user
			};

			var impersonatedUserId = _context.HttpContext.User.Claims.FirstOrDefault(a => a.Type == CustomClaimTypes.ImpersonatedUserId);
			if (impersonatedUserId != null)
			{
				_applicationUser.ImpersonatedUser = await _dbContext.Users.FirstAsync(a => a.Id == Guid.Parse(impersonatedUserId.Value));
			}
			return _applicationUser;
		}

		public async Task<User> GetAuthorizedUserAsync()
		{
			var appUser = await GetApplicationUserAsync();
			return appUser.ImpersonatedUser ?? appUser.AuthenticatedUser;
		}

		public void Dispose()
		{
			_applicationUser = null;
		}
	}
}