namespace Models.Settings
{
	public class JwtAuth
	{
		public JwtAuth()
		{
			ExpiresDays = 60;
			ValidIssuer = "yourdomain.com";
			ValidAudience = "yourdomain.com";
		}
		public string SecurityKey { get; set; }
		public int ExpiresDays { get; set; }
		public string ValidIssuer { get; set; }
		public string ValidAudience { get; set; }
	}
}
