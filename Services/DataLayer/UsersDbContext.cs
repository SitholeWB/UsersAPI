using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataLayer
{
	public class UsersDbContext : DbContext, IUsersDbContext
	{
		public UsersDbContext(DbContextOptions<UsersDbContext> options): base(options)
		{
			
		}
		public DbSet<ErrorLog> ErrorLogs { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<OAuthProvider> OAuthProviders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public async Task MigrateAsync()
		{
			await Database.MigrateAsync();
		}
	}
}
