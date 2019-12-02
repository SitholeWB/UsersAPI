using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
	public class BaseEntity
	{
		public Guid Id { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime LastModifiedDate { get; set; }
	}
}
