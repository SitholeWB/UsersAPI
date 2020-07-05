namespace Models.Constants
{
	public static class AuthType//OAuth Providers
	{
		public const string USERS_API = "USERS_API";//This USERS_API, that mean user has manually registered
		public const string FACEBOOK = "FACEBOOK";

		//To come on next version
		public const string GOOGLE = "GOOGLE";

		public const string TWITTER = "TWITTER";
		public const string LINKEDIN = "LINKEDIN";
		public const string YAHOO = "YAHOO";
		public const string MICROSOFT = "MICROSOFT";
		public const string AMAZON = "AMAZON";
	}

	public static class Status//OAuth Providers
	{
		public const string PENDING_VERRIFICATION = "PENDING_VERRIFICATION";
		public const string VERIFIED = "VERIFIED";
		public const string SUSPENDED = "SUSPENDED";
		public const string DELETED = "DELETED";
		public const string PERMANENT_DELETED = "PERMANENT_DELETED";
	}
}