﻿using Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
	public interface ISettingsService
	{
		JwtAuth GetJwtAuth();
		FacebookAuth GetFacebookAuth();
		Cryptography GetCryptography();
	}
}
