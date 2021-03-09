using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Commands;
using Models.Commands.Responses;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using Services.DataLayer;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
	public class UserRoleService : IUserRoleService
	{
		private readonly UsersDbContext _dbContext;

		public UserRoleService(UsersDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<UserRoleResponse> AddUserRoleAsync(AddUserRoleCommand command)
		{
			var role = await _dbContext.Roles.FindAsync(command.RoleId);
			if (role == null)
			{
				throw new UserException($"No Role found for given Id: {command.RoleId}.", ErrorCodes.RoleWithGivenIdNotFound);
			}
			var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == command.UserId);
			if (user == null)
			{
				throw new UserException($"No User found for given Id: {command.UserId}.", ErrorCodes.UserWithGivenIdNotFound);
			}

			var userRoleEntity = await _dbContext.UserRoles.AddAsync(new UserRole
			{
				RoleId = command.RoleId,
				DateAdded = DateTimeOffset.UtcNow,
				Id = Guid.NewGuid(),
				LastModifiedDate = DateTimeOffset.UtcNow,
				UserId = command.UserId,
				Role = role,
				User = user
			});
			await _dbContext.SaveChangesAsync();

			return UserRolesModelsHelper.ConvertUserRoleToUserRoleResponse(userRoleEntity.Entity);
		}

		public async Task DeleteUserRoleByIdAsync(Guid id)
		{
			var userRole = await _dbContext.UserRoles.FindAsync(id);
			if (userRole == null)
			{
				throw new UserException($"No UserROle found for given Id: {id}.", ErrorCodes.UserRoleWithGivenIdNotFound);
			}
			_dbContext.UserRoles.Remove(userRole);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<UserRoleResponse> GetUserRoleByIdAsync(Guid id)
		{
			return UserRolesModelsHelper.ConvertUserRoleToUserRoleResponse(await _dbContext.UserRoles.Include(i => i.Role).FirstOrDefaultAsync(a => a.Id == id));
		}

		public async Task<IEnumerable<UserRoleResponse>> GetUserRolesForUserIdAsync(Guid userId)
		{
			var userRoles = await _dbContext.UserRoles.Include(i => i.Role).Where(a => a.UserId == userId).ToListAsync();
			return userRoles?.Select(a => UserRolesModelsHelper.ConvertUserRoleToUserRoleResponse(a));
		}
	}
}