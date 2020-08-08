using Contracts;
using Models.Commands;
using Models.Constants;
using Models.DTOs.Facebook;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.OAuthProviders
{
	public static class FacebookAuthHelper
	{
		public static async Task<User> GetFacebookJwtTokeAsync(FacebookAuthViewModel model, IUsersService usersService, ISettingsService settingsService, IOAuthProviderService authProviderService, HttpClient httpClient)
		{
			//From https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login

			// 1.generate an app access token
			string appSecret = settingsService.GetFacebookAuth().AppSecret;
			string appId = settingsService.GetFacebookAuth().AppId;
			var appAccessTokenResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={appId}&client_secret={appSecret}&grant_type=client_credentials");
			var appAccessToken = JsonSerializer.Deserialize<FacebookAppAccessToken>(appAccessTokenResponse);
			// 2. validate the user access token
			var userAccessTokenValidationResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
			var userAccessTokenValidation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

			if (!userAccessTokenValidation.Data.IsValid)
			{
				throw new UserException(userAccessTokenValidation.Data?.Error?.Message ?? "Given Facebook token is invalid.", ErrorCodes.InvalidFacebookToken);
			}

			// 3. we've got a valid token so we can request user data from fb
			var userInfoResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
			var userInfo = JsonSerializer.Deserialize<FacebookUserData>(userInfoResponse);

			// 4. ready to create the local user account (if necessary) and JWT
			var user = await usersService.GetUserEntityByInputAsync(email: userInfo.Email);

			if (user == null)
			{
				var appUser = new AddUserCommand
				{
					Name = userInfo.FirstName,
					Surname = userInfo.LastName,
					Email = userInfo.Email,
					AccountAuth = AuthType.FACEBOOK,
					Gender = userInfo.Gender,
					Password = string.Empty
				};

				var result = await usersService.AddUserAsync(appUser);

				if (result == null)
				{
					throw new UserException("Failed to create user from Facebook token.", ErrorCodes.FailedToCreateUserFromFacebookToken);
				}

				await authProviderService.AddOAuthProviderAsync(new OAuthProvider
				{
					Email = userInfo.Email,
					ProviderName = AuthType.FACEBOOK,
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

			return user;
		}
	}
}