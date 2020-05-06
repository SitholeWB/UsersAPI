using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Commands;
using Models.Constants;
using Models.Entities;

namespace UsersAPI.Controllers
{
	[Authorize(Policy = Policy.EVERYONE)]
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersService _usersService;

		public UsersController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[Route("email/{email}")]
		[HttpGet]
		public async Task<ActionResult<User>> GetUserByEmailAsync(string email)
		{
			return Ok(await _usersService.GetUserByEmailAsync(email));
		}

		[Route("username/{username}")]
		[HttpGet]
		public async Task<ActionResult<User>> GetUserByUsernameAsync(string username)
		{
			return Ok(await _usersService.GetUserByUsernameAsync(username));
		}

		[Route("{id}")]
		[HttpGet]
		public async Task<ActionResult<User>> GetUserAsync(Guid id)
		{
			return Ok(await _usersService.GetUserAsync(id));
		}

		[Authorize(Policy = Policy.SUPER_ADMIN)]
		[Route("")]
		[HttpGet]
		public async Task<ActionResult<User>> GetUsersAsync()
		{
			return Ok(await _usersService.GetUsersAsync());
		}

		[Route("")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<User>> AddUserAsync([FromBody] AddUserCommand user)
		{
			return Ok(await _usersService.AddUserAsync(user));
		}

		[Route("")]
		[HttpPut]
		public async Task<ActionResult<User>> UpdateUserAsync([FromBody] User user)
		{
			//TODO: Only allow authorized user to call this endpoint
			return Ok(await _usersService.UpdateUserAsync(user));
		}


		[Authorize(Policy = Policy.SUPER_ADMIN)]
		[Route("{id}/roles")]
		[HttpPut]
		public async Task<ActionResult<User>> UpdateUserAsync([FromBody] SetUserRoleCommand role, [FromRoute] Guid id)
		{
			return Ok(await _usersService.SetUserRoleAsync(id, role));
		}

	}
}