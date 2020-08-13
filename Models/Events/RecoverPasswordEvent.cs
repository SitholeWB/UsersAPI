using Models.DTOs;
using Models.Entities;

namespace Models.Events
{
	public class RecoverPasswordEvent : EventBase
	{
		public RecoverPassword RecoverPassword { get; set; }
		public MiniUser User { get; set; }
	}
}