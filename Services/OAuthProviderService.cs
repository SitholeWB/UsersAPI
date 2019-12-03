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
	public class OAuthProviderService : IOAuthProviderService
	{
		private readonly UsersDbContext _dbContext;
		public OAuthProviderService(UsersDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<OAuthProvider> AddOAuthProviderAsync(OAuthProvider data)
		{
			data.Id = data.Id;
			data.DateAdded = DateTime.UtcNow;
			data.LastModifiedDate = DateTime.UtcNow;

			var entity = await _dbContext.AddAsync<OAuthProvider>(data);
			await _dbContext.SaveChangesAsync();
			return entity.Entity;
		}

		public async Task<OAuthProvider> GetOAuthProviderByEmailAsync(string email)
		{
			return await _dbContext.OAuthProviders?.AsNoTracking().FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());
		}

		public async Task<OAuthProvider> GetOAuthProviderByIdAsync(Guid id)
		{
			return await _dbContext.OAuthProviders?.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
		}

		public async Task<OAuthProvider> UpdateOAuthProviderAsync(OAuthProvider data)
		{
			var entity = await GetOAuthProviderByIdAsync(data.Id);
			if(entity == null || entity == default)
			{
				entity = await GetOAuthProviderByEmailAsync(data.Email);
			}
			entity.ProviderName = data.ProviderName;
			entity.LastModifiedDate = DateTime.UtcNow;
			entity.DataJson = data.DataJson;

			_dbContext.Update<OAuthProvider>(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}
	}
}
