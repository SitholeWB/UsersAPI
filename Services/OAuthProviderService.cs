using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Services.DataLayer;
using System;
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

		public async Task<OAuthProvider> AddOAuthProviderAsync(OAuthProvider authProvider)
		{
			authProvider.DateAdded = DateTimeOffset.UtcNow;
			authProvider.LastModifiedDate = DateTimeOffset.UtcNow;

			var entity = await _dbContext.AddAsync<OAuthProvider>(authProvider);
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

		public async Task<OAuthProvider> UpdateOAuthProviderAsync(OAuthProvider authProvider)
		{
			var entity = await GetOAuthProviderByIdAsync(authProvider.Id);
			if (entity == null)
			{
				entity = await GetOAuthProviderByEmailAsync(authProvider.Email);
			}
			entity.ProviderName = authProvider.ProviderName;
			entity.LastModifiedDate = DateTimeOffset.UtcNow;
			entity.DataJson = authProvider.DataJson;

			_dbContext.Update<OAuthProvider>(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}
	}
}