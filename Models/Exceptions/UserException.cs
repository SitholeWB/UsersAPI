using Models.Enums;
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
		public UserException(string message, ErrorCodes code) : base(message)
		{
			Code = $"UEX_{(int)code}";
			ErrorCode = (int)code;
		}
		public UserException(string message, ErrorCodes code, Exception exception):this(message, code)
		{
			Code = $"UEX_{(int)code}";
			Exception = exception;
			ErrorCode = (int)code;
		}
	}
}
