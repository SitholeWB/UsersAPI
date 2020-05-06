using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs.Auth
{
	public class TokenResponse
	{
		public string Token { get; set; }
		public string Role { get; set; }
		public DateTime Expire { get; set; }
		public string Name { get; set; }
	}
}
