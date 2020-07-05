using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
	[Table("ErrorLogs")]
	public class ErrorLog : BaseEntity
	{
		[MaxLength(100)]
		[Required]
		public string Type { get; set; }

		[MaxLength(5000)]
		[Required]
		public string Message { get; set; }

		[MaxLength(int.MaxValue)]
		[Required]
		public string Exception { get; set; }

		[MaxLength(500)]
		public string LocationInCode { get; set; }

		[MaxLength(int.MaxValue)]
		public string RequestDetails { get; set; }
	}
}