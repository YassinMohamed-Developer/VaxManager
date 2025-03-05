using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
		private readonly IEmailService _emailService;

		public AuthService(SignInManager<AppUser> signInManager
			, UserManager<AppUser> userManager
			,ITokenService tokenService,
			IEmailService emailService)
        {
			_signInManager = signInManager;
			_userManager = userManager;
			_tokenService = tokenService;
			_emailService = emailService;
		}


		public async Task<BaseResult<TokenDto>> LoginAsync(LoginDto loginDto)
		{
			var email = await _userManager.FindByEmailAsync(loginDto.Email);

			if(email == null)
			{
				throw new CustomException($"Email: {loginDto.Email} Not Valid") { StatusCode = (int)HttpStatusCode.BadRequest};
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

			//These Part Is Working Well But I Use Twilio Trail then i can't send Sms to someOne doesn't verfied 
			//his number in Twilio Account

			//var Message = new SmsMessageDto
			//{
			//    Body = "You have successfully registered on our website. We wish you a speedy recovery. <3!",
			//    PhoneNumber = email.PhoneNumber,
			//};

			//_smsService.Send(Message);

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
				UserName = registerDto.UserName.Trim(),
				PhoneNumber = registerDto.PhoneNumber,
			};

			var result = await _userManager.CreateAsync(appuser,registerDto.Password);

			if (result.Succeeded)
			{
				var roleresult = await _userManager.AddToRoleAsync(appuser, accountype);
				return new BaseResult<string> { Data = appuser.Id.ToString(), IsSuccess = true, Message = "Register Successfully." };
			}

			throw new CustomException($"{result}") { StatusCode = (int)HttpStatusCode.InternalServerError };
		}

		public async Task<BaseResult<string>> ResetPassword(ResetPasswordDto resetPasswordDto)
		{
			var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email!);

			if (user is null)
			{
				throw new CustomException("This Email is Invalid") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var result = await _userManager.ResetPasswordAsync(user,resetPasswordDto.token!,resetPasswordDto.Password);

			if (!result.Succeeded)
			{
				throw new CustomException("The Operation not Complete") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			return new BaseResult<string> { IsSuccess = true, Message = "Your Password is Changed" };
		}
		public async Task<BaseResult<string>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
		{
			var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

			if (user is  null)
			{
				throw new CustomException("This Email is Invalid") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			//var param = new Dictionary<string, string>
			//{
			//	{"token",token },
			//	{"email",forgotPasswordDto.Email }
			//};

			//var callback = QueryHelpers.AddQueryString("https://localhost:7024/api/auth/forgotpassword", param);

			var callback = $"https://localhost:7024/api/auth/resetpassword?userEmail={user.Email}&token={token}";

			var emailbody = $@"
								<html>
								<body>
										<h4>ResetPassword</h4>
										<p>Click Here in the Link to Reset Password</p>
										<a href='{callback}' style='text-decoration: none; color: blue;'>Reset Passowrd</a>
								</body>
								</html>";

			var message = new EmailDto
			{
				To = forgotPasswordDto.Email,
				Subject = "Reset Password Token",
				Body = emailbody,
			};

			 _emailService.SendEmail(message);

			return new BaseResult<string> { IsSuccess = true, Message = "Check Your Mail Please" };
		}
	}
}
