using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.Exceptions
{
	public class UserException : Exception
	{
		public int ErrorCode { get; set; }
		public string Code { get; set; }
		public Exception Exception { get; set; }
		public UserException(string message, int code) : base(message)
		{
			Code = $"UEX_{code}";
			ErrorCode = code;
		}
		public UserException(string message, int code, Exception exception):this(message, code)
		{
			Code = $"UEX_{code}";
			Exception = exception;
			ErrorCode = code;
		}
	}
}
