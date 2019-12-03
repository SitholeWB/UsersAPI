using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
	public interface ICryptoEngineService
	{
		string Encrypt(string plainText);
		string Decrypt(string cipherText);
	}
}
