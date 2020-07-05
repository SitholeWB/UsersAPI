using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IErrorLogService
	{
		Task AddErrorLogAsync(ErrorLog errorLog);
		Task<IEnumerable<ErrorLog>> GetErrorLogsAsync(int lastN);
	}
}
