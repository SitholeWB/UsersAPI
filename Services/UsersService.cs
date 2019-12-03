using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Services.DataLayer;
using System;
using System.Collections.Generic;
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

		public async Task<User> AddUserAsync(User user)
		{
			user.Id = Guid.NewGuid();
			user.DateAdded = DateTime.UtcNow;
			user.Email = user.Email;
			user.Username = user.Username;
			user.LastModifiedDate = DateTime.UtcNow;
			if (string.IsNullOrEmpty(user.Password) == false)
			{
				user.Password = _cryptoEngineService.Encrypt(user.Password);
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

		public async Task<User> UpdateUserAsync(User user)
		{
			var entity = await GetUserByUsernameAsync(user.Username);
			entity.Name = user.Name;
			entity.LastModifiedDate = DateTime.UtcNow;
			entity.About = user.About;
			entity.Verified = user.Verified;
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
