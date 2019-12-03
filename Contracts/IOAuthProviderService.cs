using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IOAuthProviderService
	{
		Task<OAuthProvider> AddOAuthProviderAsync(OAuthProvider user);
		Task<OAuthProvider> UpdateOAuthProviderAsync(OAuthProvider user);
		Task<OAuthProvider> GetOAuthProviderByEmailAsync(string email);
		Task<OAuthProvider> GetOAuthProviderByIdAsync(Guid id);
	}
}
