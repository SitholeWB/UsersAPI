using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Models.Commands;
using Models.Constants;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class UsersService : IUsersService
	{
		private readonly UsersDbContext _dbContext;
		private readonly ICryptoEngineService _cryptoEngineService;
		public UsersService(UsersDbContext dbContext, ICryptoEngineService cryptoEngineService)
		{
			_dbContext = dbContext;
			_cryptoEngineService = cryptoEngineService;
		}

		public async Task<User> AddUserAsync(AddUserCommand userCommand)
		{
			var user = new User
			{
				Id = Guid.NewGuid(),
				DateAdded = DateTime.UtcNow,
				Role = UserRoles.GENERAL,
				LastModifiedDate = DateTime.UtcNow,
				Name = userCommand.Name,
				Surname = userCommand.Surname,
				Email = userCommand.Email,
				Username = userCommand.Email,
				Gender = userCommand.Gender,
				Status = Status.PENDING_VERRIFICATION,
			};

			var userByEmail = await GetUserByEmailAsync(user.Email);
			if (!(userByEmail == null))
			{
				throw new UserException($"User with email '{userCommand.Email}' already exists", ErrorCodes.UserWithGivenEmailAlreadyExist);
			}
			if (string.IsNullOrEmpty(userCommand.Password) == false)
			{
				user.Password = _cryptoEngineService.Encrypt(userCommand.Password);
			}
			if (string.IsNullOrEmpty(userCommand.AccountAuth))
			{
				user.AccountAuth = AuthType.USERS_API;
			}
			else
			{
				user.AccountAuth = userCommand.AccountAuth.ToUpper();
			}

			var hasAtleastOneUser = await _dbContext.Users.AnyAsync();
			if (!hasAtleastOneUser)
			{
				user.Role = UserRoles.SUPER_ADMIN;
			}
			var entity = await _dbContext.AddAsync<User>(user);
			await _dbContext.SaveChangesAsync();
			return entity.Entity;
		}

		public async Task<User> GetUserAsync(Guid id)
		{
			return await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
		}

		public async Task<User> GetUserByEmailAsync(string email)
		{
			return await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email.ToLower());
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Username == username.ToLower());
		}

		public async Task<User> SetUserRoleAsync(Guid id, SetUserRoleCommand roleCommand)
		{
			var entity = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == id);
			entity.Role = roleCommand.Role;
			_dbContext.Update<User>(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}

		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			return await _dbContext.Users?.AsNoTracking().ToListAsync();
		}

		public async Task<User> UpdateUserAsync(User user)
		{
			var entity = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == user.Id);
			entity.Name = user.Name;
			entity.Surname = user.Surname;
			entity.LastModifiedDate = DateTime.UtcNow;
			entity.About = user.About;
			entity.Status = user.Status;
			entity.Role = user.Role;
			entity.Country = user.Country;
			if (string.IsNullOrEmpty(user.Password) == false)
			{
				entity.Password = _cryptoEngineService.Encrypt(user.Password);
			}
			_dbContext.Update<User>(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}
	}
}
