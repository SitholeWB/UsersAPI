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
	public class RoleService : IRoleService
	{
		private readonly UsersDbContext _dbContext;

		public RoleService(UsersDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<RoleResponse> AddRoleAsync(AddRoleCommand command)
		{
			if (string.IsNullOrEmpty(command?.Name))
			{
				throw new UserException("Role name is required", ErrorCodes.RoleNameIsRequired);
			}
			var roleEntity = await _dbContext.Roles.AddAsync(new Role
			{
				DateAdded = DateTimeOffset.UtcNow,
				Description = command.Description,
				Id = Guid.NewGuid(),
				LastModifiedDate = DateTimeOffset.UtcNow,
				Name = command.Name
			});
			await _dbContext.SaveChangesAsync();
			return RolesModelsHelper.ConvertRoleToRoleResponse(roleEntity.Entity);
		}

		public async Task DeleteRoleByIdAsync(Guid id)
		{
			var role = await _dbContext.Roles.FindAsync(id);
			if (role == null)
			{
				throw new UserException($"No Role found for given Id: {id}.", ErrorCodes.RoleWithGivenIdNotFound);
			}
			_dbContext.Roles.Remove(role);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<RoleResponse> GetRoleByIdAsync(Guid id)
		{
			return RolesModelsHelper.ConvertRoleToRoleResponse(await _dbContext.Roles.FindAsync(id));
		}

		public async Task<IEnumerable<RoleResponse>> GetRolesAsync()
		{
			var roles = await _dbContext.Roles.ToListAsync();
			return roles?.Select(a => RolesModelsHelper.ConvertRoleToRoleResponse(a));
		}

		public async Task<RoleResponse> UpdateRoleAsync(Guid id, UpdateRoleCommand command)
		{
			if (string.IsNullOrEmpty(command?.Name))
			{
				throw new UserException("Role name is required", ErrorCodes.RoleNameIsRequired);
			}
			var roleEntity = await _dbContext.Roles.FindAsync(id);
			if (roleEntity == null)
			{
				throw new UserException($"No Role found for given Id: {id}.", ErrorCodes.RoleWithGivenIdNotFound);
			}
			roleEntity.Name = command.Name;
			roleEntity.Description = command.Description;
			roleEntity.LastModifiedDate = DateTimeOffset.UtcNow;
			_dbContext.Roles.Update(roleEntity);
			await _dbContext.SaveChangesAsync();
			return RolesModelsHelper.ConvertRoleToRoleResponse(roleEntity);
		}
	}
}