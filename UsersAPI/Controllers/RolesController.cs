using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Commands;
using Models.Commands.Responses;
using Models.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsersAPI.Controllers
{
	[Authorize(Policy = Policy.SUPER_ADMIN)]
	[Route("api/[controller]")]
	[ApiController]
	public class RolesController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RolesController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpPost]
		public async Task<ActionResult<RoleResponse>> AddRoleAsync([FromBody] AddRoleCommand command)
		{
			return Ok(await _roleService.AddRoleAsync(command));
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<RoleResponse>> UpdateRoleAsync([FromBody] UpdateRoleCommand command, [FromRoute] Guid id)
		{
			return Ok(await _roleService.UpdateRoleAsync(id, command));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RoleResponse>>> GetRolesAsync()
		{
			return Ok(await _roleService.GetRolesAsync());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<RoleResponse>> GetRoleByIdAsync([FromRoute] Guid id)
		{
			return Ok(await _roleService.GetRoleByIdAsync(id));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRoleByIdAsync([FromRoute] Guid id)
		{
			await _roleService.DeleteRoleByIdAsync(id);
			return Ok();
		}
	}
}