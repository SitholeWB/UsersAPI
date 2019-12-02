using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.Exceptions
{
	public class UserException : Exception
	{
		public string Code { get; set; }
		[JsonIgnore]
		public Exception Exception { get; set; }
		public UserException(string message, int code) : base($"Error{message}")
		{
			Code = $"UEX_{code}";
		}
		public UserException(string message, int code, Exception exception):this(message, code)
		{
			Code = $"UEX_{code}";
			Exception = exception;			
		}
	}
}
