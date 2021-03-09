using Contracts;
using Microsoft.IdentityModel.Tokens;
using Models.Commands.Responses;
using Models.Constants;
using Models.DTOs.Auth;
using Models.DTOs.Facebook;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using Services.OAuthProviders;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
		private readonly IUserIdendityService _userIdendityService;
		private readonly HttpClient _httpClient;
		private readonly IUserRoleService _userRoleService;

		public AuthService(IUsersService usersService, ISettingsService settingsService, ICryptoEngineService cryptoEngineService,
			IOAuthProviderService authProviderService, HttpClient httpClient, IUserIdendityService userIdendityService, IUserRoleService userRoleService)
		{
			_usersService = usersService;
			_settingsService = settingsService;
			_cryptoEngineService = cryptoEngineService;
			_authProviderService = authProviderService;
			_httpClient = httpClient;
			_userIdendityService = userIdendityService;
			_userRoleService = userRoleService;
		}

		public async Task<TokenResponse> GetFacebookJwtTokeAsync(FacebookAuthViewModel model)
		{
			var user = await FacebookAuthHelper.GetFacebookJwtTokeAsync(model, _usersService, _settingsService, _authProviderService, _httpClient);
			return await GenerateJwtTokenForUser(user, AuthType.FACEBOOK);
		}

		public async Task<TokenResponse> GetJwtTokeAsync(TokenRequest tokenRequest)
		{
			var user = await _usersService.GetUserEntityByInputAsync(email: tokenRequest.Email);
			if (user == null)
			{
				throw new UserException($"Given Email \"{tokenRequest.Email}\" is not found or Password is incorrect.", ErrorCodes.GivenEmailOrPasswordIsIncorrect);
			}

			if (string.IsNullOrEmpty(tokenRequest.password))
			{
				throw new UserException("Password is required.", ErrorCodes.PasswordIsRequired);
			}

			var password = _cryptoEngineService.Decrypt(user.Password, user.Id.ToString());

			if (user.Email == tokenRequest.Email && password == tokenRequest.password)
			{
				return await GenerateJwtTokenForUser(user, AuthType.USERS_API);
			}

			throw new UserException($"Given Email \"{tokenRequest.Email}\" is not found or Password is incorrect.", ErrorCodes.GivenEmailOrPasswordIsIncorrect);
		}

		public async Task<TokenResponse> GetJwtTokenForImpersonatedUserAsync(ImpersonateTokenRequest request)
		{
			var impersonatedUser = await _usersService.GetUserEntityByInputAsync(id: request.UserId);
			if (impersonatedUser == null)
			{
				throw new UserException($"No User found for given Id: {request.UserId}.", ErrorCodes.UserWithGivenIdNotFound);
			}
			var user = await _userIdendityService.GetApplicationUserAsync();
			return await GenerateJwtTokenForUser(user.AuthenticatedUser, AuthType.IMPERSONATE, impersonatedUser);
		}

		public async Task<TokenResponse> StopJwtTokenForImpersonatedUserAsync()
		{
			var user = await _userIdendityService.GetApplicationUserAsync();
			return await GenerateJwtTokenForUser(user.AuthenticatedUser, user.AuthenticatedUser.AccountAuth);
		}

		private async Task<TokenResponse> GenerateJwtTokenForUser(User user, string accountAuth, User impersonatedUser = null)
		{
			var userId = impersonatedUser?.Id ?? user.Id;
			var userRoles = await _userRoleService.GetUserRolesForUserIdAsync(userId);
			var displayName = $"{user.Name} {user.Surname}";
			var role = user.Role;
			var claims = new List<Claim>
			{
					new Claim(ClaimTypes.AuthenticationMethod, accountAuth),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(CustomClaimTypes.Status, user.Status),
					new Claim(CustomClaimTypes.CreatedDate, DateTimeOffset.UtcNow.ToString())
			};
			var expires = DateTime.UtcNow.AddDays(_settingsService.GetJwtAuth().ExpiresDays);
			if (impersonatedUser != null)
			{
				displayName = $"Impersonated: {impersonatedUser.Name} {impersonatedUser.Surname}";
				role = impersonatedUser.Role;
				expires = DateTime.UtcNow.AddDays(1);
				claims.Add(new Claim(CustomClaimTypes.ImpersonatedUserId, impersonatedUser.Id.ToString()));
			}
			claims.Add(new Claim(ClaimTypes.Role, role));
			foreach (var userRole in userRoles ?? Enumerable.Empty<UserRoleResponse>())
			{
				claims.Add(new Claim(ClaimTypes.Role, userRole.RoleName));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingsService.GetJwtAuth().SecurityKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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
				Name = displayName
			};
		}
	}
}