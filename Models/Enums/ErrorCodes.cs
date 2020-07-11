using System.ComponentModel;

namespace Models.Enums
{
	public enum ErrorCodes
	{
		//Bad Request from 10 to 50
		[Description("Account type provided is invalid.")]
		AccountAuthInvalid = 10,

		[Description("Login failure, Invalid Facebook token.")]
		InvalidFacebookToken = 11,

		[Description("Login failure, failed to create user from Facebook token.")]
		FailedToCreateUserFromFacebookToken = 12,

		[Description("Given Email is not found or Password is incorrect.")]
		GivenEmailOrPasswordIsIncorrect = 13,

		[Description("Password is required.")]
		PasswordIsRequired = 14,

		[Description("User with given email already exist.")]
		UserWithGivenEmailAlreadyExist = 15,

		//Not Found from 51 to 100
		[Description("User with given email address does not exist on the system.")]
		UserWithGivenEmailNotFound = 51,

		[Description("User with given Username does not exist on the system.")]
		UserWithGivenUsernameNotFound = 52,

		[Description("User with given Id does not exist on the system.")]
		UserWithGivenIdNotFound = 53,

		//Forbidden from 101 to 150
		[Description("User not allowed to update information for other users.")]
		NotAllowedToUpdateOtherUserData = 101,
	}

	public enum ErrorTypes
	{
		Unexpected,
		UserException
	}
}