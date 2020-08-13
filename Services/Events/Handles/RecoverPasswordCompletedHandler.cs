using Contracts;
using Models.Events;
using System;
using System.Threading.Tasks;

namespace Services.Events.Handles
{
	public class RecoverPasswordCompletedHandler : IEventHandler<RecoverPasswordCompletedEvent>
	{
		public Task RunAsync(RecoverPasswordCompletedEvent obj)
		{
			return Task.Run(() =>
			{
				Console.WriteLine("Email to confirm success of reset password sent.");
			});
		}
	}
}