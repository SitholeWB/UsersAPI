using Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Background
{
	public sealed class RecoverPasswordTimedHostedService : IHostedService, IDisposable
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private Timer _timer;

		public RecoverPasswordTimedHostedService(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_timer = new Timer(async delegate
			{
				await DeleteExpiredRecoverPasswordsAsync();
			}, null, TimeSpan.Zero, TimeSpan.FromDays(1));

			return Task.CompletedTask;
		}

		private async Task DeleteExpiredRecoverPasswordsAsync()
		{
			var date = DateTimeOffset.UtcNow.AddHours(-24);
			using var scope = _scopeFactory.CreateScope();
			var _recoverPasswordService = scope.ServiceProvider.GetRequiredService<IRecoverPasswordService>();
			var recoverPasswords = await _recoverPasswordService.GetRecoverPasswordsBeforeDateAsync(date);
			foreach (var recoverPassword in recoverPasswords)
			{
				await _recoverPasswordService.DeleteRecoverPasswordAsync(recoverPassword.Id);
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}