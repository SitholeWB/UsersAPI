using Contracts;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
	public class CryptoEngineService : ICryptoEngineService
	{
		private readonly ISettingsService _settingsService;
		private readonly IDataProtectionProvider _provider;

		public CryptoEngineService(ISettingsService settingsService, IDataProtectionProvider provider)
		{
			_settingsService = settingsService;
			_provider = provider;
		}

		public string Encrypt(string plainText)
		{
			if (string.IsNullOrEmpty(plainText))
			{
				return string.Empty;
			}
			var protector = _provider.CreateProtector(_settingsService.GetCryptography().Key);
			return protector.Protect(plainText);
		}

		public string Decrypt(string cipherText)
		{
			if(string.IsNullOrEmpty(cipherText))
			{
				return string.Empty;
			}
			var protector = _provider.CreateProtector(_settingsService.GetCryptography().Key);
			return protector.Unprotect(cipherText);
		}
	}
}
