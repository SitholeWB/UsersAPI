using Models.Enums;
using Models.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Models.Entities
{
	[Table("Users")]
	public class User : BaseEntity
	{
		[MaxLength(350)]//https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
		[Required]
		public string Email
		{
			get { return Email_Private?.ToLower(); }
			set
			{
				Email_Private = value;
			}
		}
		[MaxLength(200)]
		[Required]
		public string Username
		{
			get { return Username_Private?.ToLower(); }
			set
			{
				Username_Private = value;
			}
		}
		[MaxLength(5000)]
		public string Password { get; set; }
		[MaxLength(200)]
		[Required]
		public string Name { get; set; }
		[MaxLength(50)]
		public string Gender { get; set; }
		[MaxLength(200)]
		[Required]
		public string Surname { get; set; }
		[MaxLength(100)]
		[Required]
		public string AccountAuth
		{
			get { return AccountAuth_Private; }
			set
			{
				if (!string.IsNullOrEmpty(value)) {
					var list = Enum.GetValues(typeof(AccountAuth)).Cast<AccountAuth>().Select(v => v.ToString()).ToList();
					if ( list.Exists(a => a.ToUpper() == value.ToUpper()) == false)
					{
						throw new UserException($"Account Auth \"{value}\" provided is invalid. It should be known by the system.", (int)ErrorCodes.AccountAuthInvalid);
					}
					AccountAuth_Private = value.ToUpper();
				}
			}
		}
		[MaxLength(200)]
		public string Country { get; set; }
		[MaxLength(int.MaxValue)]
		public string About { get; set; }
		[Required]
		public bool Verified { get; set; }

		//
		[NotMapped]
		[JsonIgnore]
		private string AccountAuth_Private { get; set; }
		[NotMapped]
		[JsonIgnore]
		private string Email_Private { get; set; }
		[NotMapped]
		[JsonIgnore]
		private string Username_Private { get; set; }
	}
}
