namespace Contracts
{
	public interface ICryptoEngineService
	{
		string Encrypt(string plainText);
		string Decrypt(string cipherText);
	}
}
