using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace VaxManager.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
        {
			_authService = authService;
		}

		[HttpPost]

		public async Task<ActionResult<BaseResult<TokenDto>>> LoginAsync(LoginDto loginDto)
		{
			var result = await _authService.LoginAsync(loginDto);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpPost]

		public async Task<ActionResult<BaseResult<string>>> RegisterPatientAsync(RegisterDto registerDto)
		{
			var result = await _authService.RegisterAsync(registerDto, "Patient");

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
		[HttpPost]
		public async Task<ActionResult<BaseResult<string>>> RegisterVaccineCenterAsync(RegisterDto registerDto)
		{
			var result = await _authService.RegisterAsync(registerDto, "VaccineCenter");

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
	}
}
