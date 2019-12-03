using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Auth;
using Models.DTOs.Facebook;

namespace UsersAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<object>> RequestToken([FromBody] TokenRequest request)
		{
			var (isSuccess, token) = await _authService.GetJwtTokeAsync(request);
			if (isSuccess)
			{
				return new { token };
			}

			return BadRequest("Could not verify Username or password");
		}

		[AllowAnonymous]
		[HttpPost("facebook")]
		public async Task<ActionResult<object>> GetFacebookJwtTokeAsync([FromBody] FacebookAuthViewModel request)
		{
			var (isSuccess, token) = await _authService.GetFacebookJwtTokeAsync(request);
			if (isSuccess)
			{
				return new { token };
			}

			return BadRequest("Could not verify Facebook user.");
		}

	}
}