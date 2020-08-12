namespace Contracts
{
	public interface ICryptoEngineService
	{
		string Encrypt(string plainText, string sharedKey = "");

		string Decrypt(string cipherText, string sharedKey = "");
	}
}