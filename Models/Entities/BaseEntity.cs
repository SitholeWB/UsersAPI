using System;

namespace Models.Entities
{
	public class BaseEntity
	{
		public Guid Id { get; set; }
		public DateTimeOffset DateAdded { get; set; }
		public DateTimeOffset LastModifiedDate { get; set; }
	}
}