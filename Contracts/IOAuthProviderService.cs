using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IOAuthProviderService
	{
		Task<OAuthProvider> AddOAuthProviderAsync(OAuthProvider authProvider);
		Task<OAuthProvider> UpdateOAuthProviderAsync(OAuthProvider authProvider);
		Task<OAuthProvider> GetOAuthProviderByEmailAsync(string email);
		Task<OAuthProvider> GetOAuthProviderByIdAsync(Guid id);
	}
}
