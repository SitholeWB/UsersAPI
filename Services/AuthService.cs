using Contracts;
using Microsoft.IdentityModel.Tokens;
using Models.Constants;
using Models.DTOs.Auth;
using Models.DTOs.Facebook;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using Services.OAuthProviders;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class AuthService : IAuthService
	{
		private readonly IUsersService _usersService;
		private readonly ISettingsService _settingsService;
		private readonly ICryptoEngineService _cryptoEngineService;
		private readonly IOAuthProviderService _authProviderService;
		private readonly HttpClient _httpClient;
		public AuthService(IUsersService usersService, ISettingsService settingsService, ICryptoEngineService cryptoEngineService, IOAuthProviderService authProviderService, HttpClient httpClient)
		{
			_usersService = usersService;
			_settingsService = settingsService;
			_cryptoEngineService = cryptoEngineService;
			_authProviderService = authProviderService;
			_httpClient = httpClient;
		}

		public async Task<TokenResponse> GetFacebookJwtTokeAsync(FacebookAuthViewModel model)
		{
			var user = await FacebookAuthHelper.GetFacebookJwtTokeAsync(model, _usersService, _settingsService, _authProviderService, _httpClient);
			return GenerateJwtTokenForUser(user, AuthType.FACEBOOK);
		}


		public async Task<TokenResponse> GetJwtTokeAsync(TokenRequest tokenRequest)
		{
			var user = await _usersService.GetUserByEmailAsync(tokenRequest.Email);
			if (user == null || user == default)
			{
				throw new UserException($"Given Email \"{tokenRequest.Email}\" is not found or Password is incorrect.", ErrorCodes.GivenEmailOrPasswordIsIncorrect);
			}

			if (string.IsNullOrEmpty(tokenRequest.password))
			{
				throw new UserException("Password is required.", ErrorCodes.PasswordIsRequired);
			}

			var password = _cryptoEngineService.Decrypt(user.Password);

			if (user.Email == tokenRequest.Email && password == tokenRequest.password)
			{
				return GenerateJwtTokenForUser(user, AuthType.USERS_API);
			}

			throw new UserException($"Given Email \"{tokenRequest.Email}\" is not found or Password is incorrect.", ErrorCodes.GivenEmailOrPasswordIsIncorrect);
		}

		private TokenResponse GenerateJwtTokenForUser(User user, string accountAuth)
		{
			var claims = new[]
			{
					new Claim("Email", user.Email),
					new Claim("Role", user.Role),
					new Claim("Status", user.Status),
					new Claim("AccountAuthType", accountAuth)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingsService.GetJwtAuth().SecurityKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var expires = DateTime.UtcNow.AddDays(_settingsService.GetJwtAuth().ExpiresDays);
			var token = new JwtSecurityToken(
				issuer: _settingsService.GetJwtAuth().ValidIssuer,
				audience: _settingsService.GetJwtAuth().ValidAudience,
				claims: claims,
				expires: expires,
				signingCredentials: creds);

			return new TokenResponse
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Role = user.Role,
				Expire = expires,
				Name = user.Name
			};
		}
	}
}
