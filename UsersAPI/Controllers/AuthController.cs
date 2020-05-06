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
		public async Task<ActionResult<TokenResponse>> RequestToken([FromBody] TokenRequest request)
		{
			return Ok(await _authService.GetJwtTokeAsync(request));
		}

		[AllowAnonymous]
		[HttpPost("facebook")]
		public async Task<ActionResult<TokenResponse>> GetFacebookJwtTokeAsync([FromBody] FacebookAuthViewModel request)
		{
			return Ok(await _authService.GetFacebookJwtTokeAsync(request));
		}

	}
}