using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities
{
	[Table("RecoverPasswords")]
	public class RecoverPassword : BaseEntity
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

		[MaxLength(5000)]
		[JsonIgnore]
		public string Hash { get; set; }

		[NotMapped]
		[JsonIgnore]
		private string Email_Private { get; set; }
	}
}