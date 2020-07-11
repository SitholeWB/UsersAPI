using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Constants;
using Models.DTOs.Auth;
using Models.DTOs.Facebook;
using System.Threading.Tasks;

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
		public async Task<ActionResult<TokenResponse>> GetFacebookJwtTokenAsync([FromBody] FacebookAuthViewModel request)
		{
			return Ok(await _authService.GetFacebookJwtTokeAsync(request));
		}

		[Authorize(Policy = Policy.SUPER_ADMIN)]
		[HttpPost("impersonate")]
		public async Task<ActionResult<TokenResponse>> GetJwtTokenForImpersonatedUserAsync([FromBody] ImpersonateTokenRequest request)
		{
			return Ok(await _authService.GetJwtTokenForImpersonatedUserAsync(request));
		}

		[Authorize(Policy = Policy.EVERYONE)]
		[HttpGet("impersonate/stop")]
		public async Task<ActionResult<TokenResponse>> StopJwtTokenForImpersonatedUserAsync()
		{
			return Ok(await _authService.StopJwtTokenForImpersonatedUserAsync());
		}
	}
}