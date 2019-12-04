using Contracts;
using Microsoft.Extensions.Options;
using Models.Settings;
using System;

namespace Services
{
	public class SettingsService : ISettingsService
	{
		private readonly JwtAuth _jwtAuth;
		private readonly FacebookAuth _facebookAuth;
		private readonly Cryptography _cryptography;

		public SettingsService(IOptions<JwtAuth> jwtAuth, IOptions<FacebookAuth> facebookAuth, IOptions<Cryptography> cryptography)
		{
			_jwtAuth = jwtAuth?.Value ?? new JwtAuth();
			_facebookAuth = facebookAuth?.Value ?? new FacebookAuth();
			_cryptography = cryptography?.Value ?? new Cryptography();
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
