using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Repository.Interface;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
	public class AuthService : IAuthService
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;

		public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,ITokenService tokenService)
        {
			_signInManager = signInManager;
			_userManager = userManager;
			_tokenService = tokenService;
		}
        public async Task<BaseResult<TokenDto>> LoginAsync(LoginDto loginDto)
		{
			var email = await _userManager.FindByEmailAsync(loginDto.Email);

			if(email == null)
			{
				throw new CustomException($"Email: {loginDto.Email} Not Valid");
			}
			var signin = await _signInManager.CheckPasswordSignInAsync(email, loginDto.Password,false);

			if(signin == null)
			{
				throw new CustomException($"Invalid Credentials for {loginDto.Email}") {StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var MakeToken = new TokenDto
			{
				TokenType = "bearer",
				Token = await _tokenService.GenerateToken(email)
			};

			return new BaseResult<TokenDto> { Data = MakeToken,Message = "Login Successfully." };
		}

		public async Task<BaseResult<string>> RegisterAsync(RegisterDto registerDto, string accountype)
		{
			var email = await _userManager.FindByEmailAsync(registerDto.Email);

			if (email is not null)
			{
				throw new CustomException($"Email {registerDto.Email} is Already Exist") {StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var username = await _userManager.FindByNameAsync(registerDto.UserName);
			if(username is not null)
			{
				throw new CustomException($"UserName {registerDto.UserName} is Already Exist");
			}

			var appuser = new AppUser
			{
				Email = registerDto.Email,
				UserName = registerDto.UserName,
				PhoneNumber = registerDto.PhoneNumber,
			};

			var result = await _userManager.CreateAsync(appuser,registerDto.Password);

			if (result.Succeeded)
			{
				var roleresult = await _userManager.AddToRoleAsync(appuser, accountype);
				return new BaseResult<string> { Data = appuser.Id.ToString(), IsSuccess = true, Message = "Register Successfully." };
			}

			throw new CustomException($"{result.Errors}") { StatusCode = (int)HttpStatusCode.InternalServerError };
		}
	}
}
