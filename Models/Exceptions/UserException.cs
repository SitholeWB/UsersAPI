using Models.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.Exceptions
{
	[Serializable]
	public class UserException : Exception, ISerializable
	{
		public int ErrorCode { get; set; }
		public string Code { get; set; }
		public Exception Exception { get; set; }

		public UserException(string message, ErrorCodes code) : base(message)
		{
			Code = $"UEX_{(int)code}";
			ErrorCode = (int)code;
		}

		public UserException(string message, ErrorCodes code, Exception exception) : this(message, code)
		{
			Code = $"UEX_{(int)code}";
			Exception = exception;
			ErrorCode = (int)code;
		}

		protected UserException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}