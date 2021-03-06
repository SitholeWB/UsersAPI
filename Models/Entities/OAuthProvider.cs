﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
	[Table("OAuthProviders")]
	public class OAuthProvider : BaseEntity // Id on this table should be the User Id
	{
		public Guid UserId { get; set; }

		[MaxLength(100)]
		[Required]
		public string ProviderName { get; set; }

		[MaxLength(350)]//https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
		[Required]
		public string Email { get; set; }

		[MaxLength(int.MaxValue)]
		public string DataJson { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
	}
}