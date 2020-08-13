using Contracts;
using Models.Events;
using System;
using System.Threading.Tasks;

namespace Services.Events.Handles
{
	public class RecoverPasswordSendEmailHandler : IEventHandler<RecoverPasswordEvent>
	{
		public Task RunAsync(RecoverPasswordEvent obj)
		{
			return Task.Run(() =>
			{
				Console.WriteLine("Email to reset password sent.");
			});
		}
	}
}