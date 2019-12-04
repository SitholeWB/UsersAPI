using Contracts;
using Microsoft.IdentityModel.Tokens;
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

		public async Task<string> GetFacebookJwtTokeAsync(FacebookAuthViewModel model)
		{
			var user = await FacebookAuthHelper.GetFacebookJwtTokeAsync(model, _usersService, _settingsService, _authProviderService, _httpClient);
			return GenerateJwtTokenForUser(user, AccountAuth.FACEBOOK);
		}


		public async Task<string> GetJwtTokeAsync(TokenRequest tokenRequest)
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
				return GenerateJwtTokenForUser(user, AccountAuth.USERSAPI);
			}

			throw new UserException($"Given Email \"{tokenRequest.Email}\" is not found or Password is incorrect.", ErrorCodes.GivenEmailOrPasswordIsIncorrect);
		}

		private string GenerateJwtTokenForUser(User user, AccountAuth accountAuth)
		{
			var claims = new[]
			{
					new Claim("Email", user.Email),
					new Claim(type: "AuthenticationMethod", accountAuth.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingsService.GetJwtAuth().SecurityKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _settingsService.GetJwtAuth().ValidIssuer,
				audience: _settingsService.GetJwtAuth().ValidAudience,
				claims: claims,
				expires: DateTime.Now.AddDays(_settingsService.GetJwtAuth().ExpiresDays),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
