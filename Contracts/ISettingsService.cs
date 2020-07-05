using Models.Settings;

namespace Contracts
{
	public interface ISettingsService
	{
		JwtAuth GetJwtAuth();
		FacebookAuth GetFacebookAuth();
		Cryptography GetCryptography();
	}
}
