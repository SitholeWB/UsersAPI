using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ErrorLogService : IErrorLogService
	{
		private readonly UsersDbContext _dbContext;

		public ErrorLogService(UsersDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddErrorLogAsync(ErrorLog errorLog)
		{
			errorLog.Id = Guid.NewGuid();
			errorLog.DateAdded = DateTime.UtcNow;
			errorLog.LastModifiedDate = DateTime.UtcNow;
			await _dbContext.AddAsync<ErrorLog>(errorLog);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<ErrorLog>> GetErrorLogsAsync(int lastN)
		{
			return await _dbContext.ErrorLogs?.OrderByDescending(a => a.DateAdded).Take(lastN)?.ToListAsync();
		}
	}
}
