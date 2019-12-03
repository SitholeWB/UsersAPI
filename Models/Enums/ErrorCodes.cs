using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models.Enums
{
	public enum ErrorCodes
	{
		//Bad Request from 10 to 50
		[Description("Account type provided is invalid.")]
		AccountAuthInvalid = 10,

		//Not Found from 51 to 100
		[Description("User with given email address does not exist on the system.")]
		UserWithGivenEmailNotFound = 51,
		[Description("User with given Username does not exist on the system.")]
		UserWithGivenUsernameNotFound = 52,
	}

	public enum ErrorTypes
	{
		Unexpected,
		UserException
	}
}
