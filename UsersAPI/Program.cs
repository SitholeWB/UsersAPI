using Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Background;
using System;
using System.Threading.Tasks;

namespace UsersAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			using (var serviceScope = host.Services.CreateScope())
			{
				var services = serviceScope.ServiceProvider;

				try
				{
					var usersDbContext = services.GetRequiredService<IUsersDbContext>();
					await usersDbContext.MigrateAsync();
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred.");
				}
			}

			await host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.ConfigureKestrel(serverOptions =>
					{
						// Set properties and call methods on options
					}).UseStartup<Startup>();
				}).ConfigureServices(services =>
				{
					services.AddHostedService<RecoverPasswordTimedHostedService>();
					services.AddHostedService<QueuedHostedService>();
				});
	}
}