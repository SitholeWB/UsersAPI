using Models.DTOs;
using Models.Entities;

namespace Models.Events
{
	public class RecoverPasswordCompletedEvent : EventBase
	{
		public RecoverPassword RecoverPassword { get; set; }
		public MiniUser User { get; set; }
	}
}