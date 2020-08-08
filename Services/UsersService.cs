using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Commands;
using Models.Commands.Responses;
using Models.Constants;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
	public class UsersService : IUsersService
	{
		private readonly UsersDbContext _dbContext;
		private readonly ICryptoEngineService _cryptoEngineService;
		private readonly IUserIdendityService _userIdendityService;

		public UsersService(UsersDbContext dbContext, ICryptoEngineService cryptoEngineService, IUserIdendityService userIdendityService)
		{
			_dbContext = dbContext;
			_cryptoEngineService = cryptoEngineService;
			_userIdendityService = userIdendityService;
		}

		public async Task<UserResponse> AddUserAsync(AddUserCommand userCommand)
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
			if (userByEmail != null)
			{
				throw new UserException($"User with email '{userCommand.Email}' already exists", ErrorCodes.UserWithGivenEmailAlreadyExist);
			}
			if (!string.IsNullOrEmpty(userCommand.Password))
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
			return ConvertUserToUserResponse(entity.Entity);
		}

		public async Task<UserResponse> GetUserAsync(Guid id)
		{
			return ConvertUserToUserResponse(await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id));
		}

		public async Task<UserResponse> GetUserByEmailAsync(string email)
		{
			return ConvertUserToUserResponse(await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email.ToLower()));
		}

		public async Task<UserResponse> GetUserByUsernameAsync(string username)
		{
			return ConvertUserToUserResponse(await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Username == username.ToLower()));
		}

		public async Task<UserResponse> SetUserRoleAsync(Guid id, SetUserRoleCommand roleCommand)
		{
			var entity = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == id);
			entity.Role = roleCommand.Role;
			_dbContext.Update<User>(entity);
			await _dbContext.SaveChangesAsync();
			return ConvertUserToUserResponse(entity);
		}

		public async Task<IEnumerable<UserResponse>> GetUsersAsync()
		{
			return (await _dbContext.Users?.AsNoTracking().ToListAsync()).Select(a => ConvertUserToUserResponse(a));
		}

		public async Task<User> GetUserEntityByInputAsync(string email = null, string username = null, Guid? id = null)
		{
			User user = null;
			if (!string.IsNullOrEmpty(email))
			{
				user = await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email.ToLower());
			}

			if (user == null && !string.IsNullOrEmpty(username))
			{
				user = await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Username == username.ToLower());
			}

			if (user == null && id != null)
			{
				user = await _dbContext.Users?.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
			}

			return user;
		}

		public async Task<UserResponse> UpdateUserAsync(User user)
		{
			var authUser = await _userIdendityService.GetAuthorizedUser();
			if (user.Id != authUser.Id && authUser.Role != UserRoles.ADMIN && authUser.Role != UserRoles.SUPER_ADMIN)
			{
				throw new UserException("You are not allowed to update someone else account details.", ErrorCodes.NotAllowedToUpdateOtherUserData);
			}

			var entity = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == user.Id);
			if (entity == null)
			{
				throw new UserException($"No User found for given Id: {user.Id}.", ErrorCodes.UserWithGivenIdNotFound);
			}
			entity.Name = user.Name;
			entity.Surname = user.Surname;
			entity.LastModifiedDate = DateTime.UtcNow;
			entity.About = user.About;
			entity.Status = user.Status;
			entity.Role = user.Role;
			entity.Country = user.Country;
			if (!string.IsNullOrEmpty(user.Password))
			{
				entity.Password = _cryptoEngineService.Encrypt(user.Password);
			}
			_dbContext.Update<User>(entity);
			await _dbContext.SaveChangesAsync();
			return ConvertUserToUserResponse(entity);
		}

		#region private methods

		private static UserResponse ConvertUserToUserResponse(User userEntity)
		{
			if (userEntity == null)
			{
				return null;
			}
			return new UserResponse
			{
				About = userEntity.About,
				AccountAuth = userEntity.AccountAuth,
				Country = userEntity.Country,
				DateAdded = userEntity.DateAdded,
				Email = userEntity.Email,
				Gender = userEntity.Gender,
				Id = userEntity.Id,
				LastModifiedDate = userEntity.LastModifiedDate,
				Name = userEntity.Name,
				Role = userEntity.Role,
				Status = userEntity.Status,
				Surname = userEntity.Surname,
				Username = userEntity.Username,
			};
		}

		#endregion private methods
	}
}