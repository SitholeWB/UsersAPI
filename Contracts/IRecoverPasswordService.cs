using Models.Commands;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IRecoverPasswordService
	{
		Task TriggerForgottenPasswordAsync(ForgettenPasswordCommand command);

		Task SetNewUserPasswordAsync(SetNewUserPasswordCommand command, Guid id, string hash);

		Task<RecoverPassword> GetRecoverPasswordAsync(Guid id);

		Task DeleteRecoverPasswordAsync(Guid id);

		Task<IEnumerable<RecoverPassword>> GetRecoverPasswordsBeforeDateAsync(DateTimeOffset beforeDate);
	}
}