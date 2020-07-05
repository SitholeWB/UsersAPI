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
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("email")]
		public string Email
		{
			get { return Email_Private?.ToLower(); }
			set
			{
				Email_Private = value;
			}
		}

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }

		[JsonPropertyName("last_name")]
		public string LastName { get; set; }

		[JsonPropertyName("gender")]
		public string Gender { get; set; }

		public string Locale { get; set; }

		[JsonPropertyName("picture")]
		public FacebookPictureData Picture { get; set; }

		[JsonIgnore]
		private string Email_Private { get; set; }
	}

	public class FacebookPictureData
	{
		[JsonPropertyName("data")]
		public FacebookPicture Data { get; set; }
	}

	public class FacebookPicture
	{
		[JsonPropertyName("height")]
		public int Height { get; set; }

		[JsonPropertyName("width")]
		public int Width { get; set; }

		[JsonPropertyName("is_silhouette")]
		public bool IsSilhouette { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }
	}

	public class FacebookUserAccessTokenData
	{
		[JsonPropertyName("app_id")]
		public string AppId { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("application")]
		public string Application { get; set; }

		[JsonPropertyName("expires_at")]
		public long ExpiresAt { get; set; }

		[JsonPropertyName("is_valid")]
		public bool IsValid { get; set; }

		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		[JsonPropertyName("scopes")]
		public string[] Scopes { get; set; }

		[JsonPropertyName("error")]
		public Error Error { get; set; }
	}

	public class Error
	{
		[JsonPropertyName("code")]
		public int Code { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("subcode")]
		public int Subcode { get; set; }
	}

	public class FacebookUserAccessTokenValidation
	{
		[JsonPropertyName("data")]
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