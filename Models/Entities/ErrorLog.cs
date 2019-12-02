using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
	[Table("ErrorLogs")]
	public class ErrorLog : BaseEntity
	{
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
