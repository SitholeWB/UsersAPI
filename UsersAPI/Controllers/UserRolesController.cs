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
	[Authorize(Policy = Policy.ALL_ADMINS)]
	[Route("api/user-roles")]
	[ApiController]
	public class UserRolesController : ControllerBase
	{
		private readonly IUserRoleService _userRoleService;

		public UserRolesController(IUserRoleService userRoleService)
		{
			_userRoleService = userRoleService;
		}

		[HttpPost]
		public async Task<ActionResult<UserRoleResponse>> AddRoleAsync([FromBody] AddUserRoleCommand command)
		{
			return Ok(await _userRoleService.AddUserRoleAsync(command));
		}

		[HttpGet("role/{roleId}")]
		public async Task<ActionResult<IEnumerable<RoleResponse>>> GetRolesAsync([FromRoute] Guid roleId)
		{
			return Ok(await _userRoleService.GetUserRoleByIdAsync(roleId));
		}

		[HttpGet("user/{userId}")]
		public async Task<ActionResult<RoleResponse>> GetRoleByIdAsync([FromRoute] Guid userId)
		{
			return Ok(await _userRoleService.GetUserRolesForUserIdAsync(userId));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> DeleteUserRoleByIdAsync([FromRoute] Guid id)
		{
			await _userRoleService.DeleteUserRoleByIdAsync(id);
			return Ok();
		}
	}
}