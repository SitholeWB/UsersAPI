using System;

namespace Models.DTOs.Auth
{
	public class ImpersonateTokenRequest
	{
		public Guid UserId { get; set; }
	}
}