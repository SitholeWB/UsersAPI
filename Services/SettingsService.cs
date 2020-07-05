using Contracts;
using Models.Settings;

namespace Services
{
	public class SettingsService : ISettingsService
	{
		private readonly JwtAuth _jwtAuth;
		private readonly FacebookAuth _facebookAuth;
		private readonly Cryptography _cryptography;

		public SettingsService(JwtAuth jwtAuth, FacebookAuth facebookAuth, Cryptography cryptography)
		{
			_jwtAuth = jwtAuth ?? new JwtAuth();
			_facebookAuth = facebookAuth ?? new FacebookAuth();
			_cryptography = cryptography ?? new Cryptography();
		}

		public Cryptography GetCryptography()
		{
			return _cryptography;
		}

		public FacebookAuth GetFacebookAuth()
		{
			return _facebookAuth;
		}

		public JwtAuth GetJwtAuth()
		{
			return _jwtAuth;
		}
	}
}