using Models.DTOs.Auth;
using Models.DTOs.Facebook;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IAuthService
	{
		Task<TokenResponse> GetJwtTokeAsync(TokenRequest tokenRequest);

		Task<TokenResponse> GetFacebookJwtTokeAsync(FacebookAuthViewModel model);

		Task<TokenResponse> GetJwtTokenForImpersonatedUserAsync(ImpersonateTokenRequest request);

		Task<TokenResponse> StopJwtTokenForImpersonatedUserAsync();
	}
}