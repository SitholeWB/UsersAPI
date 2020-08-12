using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Threading.Tasks;

namespace Services.DataLayer
{
	public class UsersDbContext : DbContext, IUsersDbContext
	{
		public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
		{
		}

		public DbSet<ErrorLog> ErrorLogs { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<OAuthProvider> OAuthProviders { get; set; }
		public DbSet<RecoverPassword> RecoverPasswords { get; set; }

		public async Task MigrateAsync()
		{
			await Database.MigrateAsync();
		}
	}
}