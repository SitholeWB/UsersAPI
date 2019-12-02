using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IUsersDbContext : IDisposable
	{
		DbSet<ErrorLog> ErrorLogs { get; set; }
		DbSet<User> Users { get; set; }
		DbSet<OAuthProvider> OAuthProviders { get; set; }
		Task MigrateAsync();
	}
}
