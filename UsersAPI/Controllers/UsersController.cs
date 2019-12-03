using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace UsersAPI.Controllers
{
	[Authorize]
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

		[Route("")]
		[HttpPost]
		public async Task<ActionResult<User>> AddUserAsync([FromBody] User user)
		{
			return Ok(await _usersService.AddUserAsync(user));
		}

		[Route("")]
		[HttpPut]
		public async Task<ActionResult<User>> UpdateUserAsync([FromBody] User user)
		{
			return Ok(await _usersService.UpdateUserAsync(user));
		}

	}
}