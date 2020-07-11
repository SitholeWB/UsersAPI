using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Auth
{
	public class ImpersonateTokenRequest
	{
		[Required]
		public Guid UserId { get; set; }
	}
}