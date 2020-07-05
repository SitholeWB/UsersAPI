using System.Text.Json.Serialization;

namespace Models.DTOs.Auth
{
	public class TokenRequest
	{
		public string Email
		{
			get { return Email_Private?.ToLower(); }
			set
			{
				Email_Private = value;
			}
		}
		public string password { get; set; }

		[JsonIgnore]
		private string Email_Private { get; set; }
	}
}
