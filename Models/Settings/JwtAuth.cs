using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Settings
{
	public class JwtAuth
	{
		public JwtAuth()
		{
			ExpiresDays = 60;
		}
		public string SecurityKey { get; set; }
		public int ExpiresDays { get; set; }
	}
}
