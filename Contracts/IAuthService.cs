using Models.DTOs.Auth;
using Models.DTOs.Facebook;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IAuthService
	{
		Task<(bool, string)> GetJwtTokeAsync(TokenRequest tokenRequest);
		Task<(bool, string)> GetFacebookJwtTokeAsync(FacebookAuthViewModel model);
	}
}
