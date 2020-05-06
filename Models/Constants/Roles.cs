using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Constants
{
	public static class UserRoles
	{
		public const string ADMIN = "ADMIN";
		public const string GENERAL = "GENERAL";
		public const string DEVELOPER = "DEVELOPER";
		public const string SUPER_ADMIN = "SUPER_ADMIN";
	}

	public static class Policy
	{
		public const string ALL_ADMINS = "ALL_ADMINS";
		public const string SUPER_ADMIN = "SUPER_ADMIN";
		public const string DEVELOPER = "DEVELOPER";
		public const string ADMIN = "ADMIN";
		public const string EVERYONE = "EVERYONE";
	}
}
