using Contracts;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Auth;
using Models.DTOs.Facebook;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
	public class AuthService : IAuthService
	{
		private readonly IUsersService _usersService;
		private readonly ISettingsService _settingsService;
		private readonly ICryptoEngineService _cryptoEngineService;
		private readonly IOAuthProviderService _authProviderService;
		public AuthService(IUsersService usersService, ISettingsService settingsService, ICryptoEngineService cryptoEngineService, IOAuthProviderService authProviderService)
		{
			_usersService = usersService;
			_settingsService = settingsService;
			_cryptoEngineService = cryptoEngineService;
			_authProviderService = authProviderService;
		}

		public async Task<(bool, string)> GetFacebookJwtTokeAsync(FacebookAuthViewModel model)
		{


			using (var httpClient = new HttpClient())
			{
				// 1.generate an app access token
				var appAccessTokenResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_settingsService.GetFacebookAuth().AppId}&client_secret={_settingsService.GetFacebookAuth().AppSecret}&grant_type=client_credentials");
				var appAccessToken = JsonSerializer.Deserialize<FacebookAppAccessToken>(appAccessTokenResponse);
				// 2. validate the user access token
				var userAccessTokenValidationResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
				var userAccessTokenValidation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);
				
				if (!userAccessTokenValidation.Data.IsValid)
				{
					return (false, "login_failure, Invalid Facebook token.");
				}

				// 3. we've got a valid token so we can request user data from fb
				var userInfoResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
				var userInfo = JsonSerializer.Deserialize<FacebookUserData>(userInfoResponse);

				// 4. ready to create the local user account (if necessary) and jwt
				var user = await _usersService.GetUserByEmailAsync(userInfo.Email);

				if (user == null || user == default)
				{
					var appUser = new User
					{
						Name = userInfo.FirstName,
						Surname = userInfo.LastName,
						Email = userInfo.Email,
						Username = userInfo.Email,
						Verified = true,
						AccountAuth = AccountAuth.FACEBOOK.ToString(),
						Gender = userInfo.Gender
						//FacebookUserData = userInfo
					};

					var result = await _usersService.AddUserAsync(appUser);

					if (result == null)
					{
						return (false, "login_failure, failed to create user.");
					}

					await _authProviderService.AddOAuthProviderAsync(new OAuthProvider
					{
						Email = user.Email,
						ProviderName = AccountAuth.FACEBOOK.ToString(),
						Id = result.Id,
						DataJson = JsonSerializer.Serialize(new FacebookDto
						{
							FacebookUserData = userInfo,
							FacebookUserAccessTokenValidation = userAccessTokenValidation,
							FacebookAppAccessToken = appAccessToken,
							FacebookAuthViewModel = model

						})
					});
				}

				// generate the jwt for the local user...
				var localUser = await _usersService.GetUserByEmailAsync(userInfo.Email);

				if (localUser == null)
				{
					return (false, "login_failure, failed to create user.");
				}

				var claims = new[]
				{
					new Claim(type: ClaimTypes.Name, localUser.Username)
				};

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingsService.GetJwtAuth().SecurityKey));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(
					issuer: "yourdomain.com",
					audience: "yourdomain.com",
					claims: claims,
					expires: DateTime.Now.AddDays(_settingsService.GetJwtAuth().ExpiresDays),
					signingCredentials: creds);

				return (true, new JwtSecurityTokenHandler().WriteToken(token));
			}

		}


		public async Task<(bool, string)> GetJwtTokeAsync(TokenRequest tokenRequest)
		{
			var user = await _usersService.GetUserByEmailAsync(tokenRequest.Email);
			if (user == null || user == default)
			{
				return (false, null);
			}
			var password = "";

			if (string.IsNullOrEmpty(user.Password) == false)
			{
				password = _cryptoEngineService.Decrypt(user.Password);
			}

			if (user.Email == tokenRequest.Email && password == tokenRequest.password)
			{
				var claims = new[]
				{
					new Claim(ClaimTypes.Name, user.Username)
				};

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingsService.GetJwtAuth().SecurityKey));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(
					issuer: "yourdomain.com",
					audience: "yourdomain.com",
					claims: claims,
					expires: DateTime.Now.AddDays(_settingsService.GetJwtAuth().ExpiresDays),
					signingCredentials: creds);

				return (true, new JwtSecurityTokenHandler().WriteToken(token));
			}

			return (false, null);
		}
	}
}
