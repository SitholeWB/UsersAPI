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
		public string Name { get; set; }
		[MaxLength(int.MaxValue)]
		public string DataJson { get; set; }
	}
}
