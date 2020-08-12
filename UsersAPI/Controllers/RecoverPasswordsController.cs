using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Commands;
using System;
using System.Threading.Tasks;

namespace UsersAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RecoverPasswordsController : ControllerBase
	{
		private readonly IRecoverPasswordService _recoverPasswordService;

		public RecoverPasswordsController(IRecoverPasswordService recoverPasswordService)
		{
			_recoverPasswordService = recoverPasswordService;
		}

		[Route("")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> TriggerForgottenPasswordAsync([FromBody] ForgettenPasswordCommand command)
		{
			await _recoverPasswordService.TriggerForgottenPasswordAsync(command);
			return Ok();
		}

		[Route("reset/{id}")]
		[HttpPut]
		[AllowAnonymous]
		public async Task<ActionResult> SetNewUserPasswordAsync([FromBody] SetNewUserPasswordCommand command, [FromRoute] Guid id, [FromQuery] string hash)
		{
			await _recoverPasswordService.SetNewUserPasswordAsync(command, id, hash);
			return Ok();
		}
	}
}