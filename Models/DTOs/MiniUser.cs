using System;

namespace Models.DTOs
{
	public class MiniUser
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string Fullnames { get; set; }
	}
}