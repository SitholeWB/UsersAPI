using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
	[Table("OAuthProviders")]
	public class OAuthProvider: BaseEntity // Id on this table should be the User Id
	{
		[MaxLength(100)]
		[Required]
		public string ProviderName { get; set; }
		[MaxLength(350)]//https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
		[Required]
		public string Email { get; set; }
		[MaxLength(int.MaxValue)]
		public string DataJson { get; set; }
	}
}
