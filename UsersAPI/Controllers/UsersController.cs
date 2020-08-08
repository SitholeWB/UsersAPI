using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Commands;
using Models.Commands.Responses;
using Models.Constants;
using Models.Entities;
using System;
using System.Threading.Tasks;

namespace UsersAPI.Controllers
{
	[Authorize(Policy = Policy.EVERYONE)]
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersService _usersService;
		private readonly IUserIdendityService _userIdendityService;

		public UsersController(IUsersService usersService, IUserIdendityService userIdendityService)
		{
			_usersService = usersService;
			_userIdendityService = userIdendityService;
		}

		[Route("email/{email}")]
		[HttpGet]
		public async Task<ActionResult<UserResponse>> GetUserByEmailAsync(string email)
		{
			return Ok(await _usersService.GetUserByEmailAsync(email));
		}

		[Route("username/{username}")]
		[HttpGet]
		public async Task<ActionResult<UserResponse>> GetUserByUsernameAsync(string username)
		{
			return Ok(await _usersService.GetUserByUsernameAsync(username));
		}

		[Route("{id}")]
		[HttpGet]
		public async Task<ActionResult<UserResponse>> GetUserAsync(Guid id)
		{
			return Ok(await _usersService.GetUserAsync(id));
		}

		[Route("mydetails")]
		[HttpGet]
		public async Task<ActionResult<UserResponse>> GetAuthorizedUser()
		{
			return Ok(await _userIdendityService.GetAuthorizedUser());
		}

		[Authorize(Policy = Policy.ALL_ADMINS)]
		[Route("")]
		[HttpGet]
		public async Task<ActionResult<UserResponse>> GetUsersAsync()
		{
			return Ok(await _usersService.GetUsersAsync());
		}

		[Route("")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<UserResponse>> AddUserAsync([FromBody] AddUserCommand user)
		{
			return Ok(await _usersService.AddUserAsync(user));
		}

		[Route("")]
		[HttpPut]
		public async Task<ActionResult<UserResponse>> UpdateUserAsync([FromBody] User user)
		{
			return Ok(await _usersService.UpdateUserAsync(user));
		}

		[Authorize(Policy = Policy.SUPER_ADMIN)]
		[Route("{id}/roles")]
		[HttpPut]
		public async Task<ActionResult<UserResponse>> UpdateUserAsync([FromBody] SetUserRoleCommand role, [FromRoute] Guid id)
		{
			return Ok(await _usersService.SetUserRoleAsync(id, role));
		}
	}
}