using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models.DTOs.Facebook
{
	public class FacebookDto
	{
		public FacebookUserData FacebookUserData { get; set; }
		public FacebookPictureData FacebookPictureData { get; set; }
		public FacebookPicture FacebookPicture { get; set; }
		public FacebookUserAccessTokenData FacebookUserAccessTokenData { get; set; }
		public FacebookUserAccessTokenValidation FacebookUserAccessTokenValidation { get; set; }
		public FacebookAppAccessToken FacebookAppAccessToken { get; set; }
		public FacebookAuthViewModel FacebookAuthViewModel { get; set; }
	}
	public class FacebookUserData
	{
		public long Id { get; set; }
		public string Email
		{
			get { return Email_Private?.ToLower(); }
			set
			{
				Email_Private = value;
			}
		}
		public string Name { get; set; }
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }
		public string Gender { get; set; }
		public string Locale { get; set; }
		public FacebookPictureData Picture { get; set; }

		[JsonIgnore]
		private string Email_Private { get; set; }
	}

	public class FacebookPictureData
	{
		public FacebookPicture Data { get; set; }
	}

	public class FacebookPicture
	{
		public int Height { get; set; }
		public int Width { get; set; }
		[JsonPropertyName("is_silhouette")]
		public bool IsSilhouette { get; set; }
		public string Url { get; set; }
	}

	public class FacebookUserAccessTokenData
	{
		[JsonPropertyName("app_id")]
		public long AppId { get; set; }
		public string Type { get; set; }
		public string Application { get; set; }
		[JsonPropertyName("expires_at")]
		public long ExpiresAt { get; set; }
		[JsonPropertyName("is_valid")]
		public bool IsValid { get; set; }
		[JsonPropertyName("user_id")]
		public long UserId { get; set; }
	}

	public class FacebookUserAccessTokenValidation
	{
		public FacebookUserAccessTokenData Data { get; set; }
	}

	public class FacebookAppAccessToken
	{
		[JsonPropertyName("token_type")]
		public string TokenType { get; set; }
		[JsonPropertyName("access_token")]
		public string AccessToken { get; set; }
	}
}
