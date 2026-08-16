using Azure.Messaging;
using Google.Apis.Auth;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using Vax.Service.Shared;

namespace Vax.Service.Implmentation
{
	public class AuthService : IAuthService
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ILogger<AuthService> _logger;
		private readonly ILocalizationService _localization;

		public AuthService(SignInManager<AppUser> signInManager
			, UserManager<AppUser> userManager
			,ITokenService tokenService,
			IEmailService emailService,
			IConfiguration configuration,
			IBackgroundJobClient backgroundJobClient,
			ILogger<AuthService> logger,
			ILocalizationService localization)
        {
			_signInManager = signInManager;
			_userManager = userManager;
			_tokenService = tokenService;
			_emailService = emailService;
			_configuration = configuration;
			_backgroundJobClient = backgroundJobClient;
			_logger = logger;
			_localization = localization;
		}


		public async Task<BaseResult<TokenDto>> LoginAsync(LoginDto loginDto)
		{
			var email = await _userManager.FindByEmailAsync(loginDto.Email);

			if(email == null)
			{
				_logger.LogError("Invalid email provided: {Email}", loginDto.Email);

				throw new CustomException(_localization.Get(ValidationError.AuthError.InvalidEmail)) { StatusCode = (int)HttpStatusCode.BadRequest};
			}
			var signin = await _signInManager.CheckPasswordSignInAsync(email, loginDto.Password,false);

			if(signin == null)
			{
				_logger.LogError("Invalid credentials provided for email: {Email}", loginDto.Email);
				throw new CustomException(_localization.Get(ValidationError.AuthError.InvalidCredentials)) { StatusCode = (int)HttpStatusCode.BadRequest };
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


			return new BaseResult<TokenDto> { Data = MakeToken,Message = _localization.Get(ValidationError.AuthError.LoginSucceeded) };
		}



		public async Task<BaseResult<string>> RegisterAsync(RegisterDto registerDto, string accountype)
		{
			var email = await _userManager.FindByEmailAsync(registerDto.Email);

			if (email is not null)
			{
				_logger.LogError("Email {Email} is already in use.", registerDto.Email);
				return new BaseResult<string>($"{_localization.Get(ValidationError.AuthError.EmailAlreadyExists)}:{registerDto.Email}") {StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var username = await _userManager.FindByNameAsync(registerDto.UserName);
			if(username is not null)
			{
				return new BaseResult<string>($"{_localization.Get(ValidationError.AuthError.UserNameAlreadyExists)}:{registerDto.UserName}") {StatusCode = (int)HttpStatusCode.BadRequest };
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
				_logger.LogInformation($"User {appuser.UserName} registered successfully with role {accountype}.");
				return new BaseResult<string> { Data = appuser.Id.ToString(), IsSuccess = true, Message = _localization.Get(ValidationError.AuthError.RegistrationSucceeded) };
			}

			var emailmessage = new EmailDto
			{
				Subject = "Welcome to Vax",
				Body = $"<h3>Dear {registerDto.UserName},</h3>" +
				$"<p>Thank you for registering with Vax. We are excited to have you on board!</p>" +
				$"<p>Your account has been successfully created, and you can now log in using your email address: {registerDto.Email}.</p>" +
				$"<p>If you have any questions or need assistance, please feel free to reach out to our support team.</p>" +
				$"<p>Best regards,<br/>The Vax Team</p>",

				To= registerDto.Email,
			};

			_backgroundJobClient.Enqueue(() => _emailService.SendEmail(emailmessage));

			_logger.LogError("User registration failed for {UserName}. Errors: {Errors}", registerDto.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
			return new  BaseResult<string> ($"{result}") { StatusCode = (int)HttpStatusCode.InternalServerError };
		}

		public async Task<BaseResult<string>> ResetPassword(ResetPasswordDto resetPasswordDto)
		{
			var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email!);

			if (user is null)
			{
				return new BaseResult<string>($"{_localization.Get(ValidationError.AuthError.InvalidEmail)}:{resetPasswordDto.Email}") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var result = await _userManager.ResetPasswordAsync(user,resetPasswordDto.token!,resetPasswordDto.Password);

			if (!result.Succeeded)
			{
				return new BaseResult<string>($"{_localization.Get(ValidationError.AuthError.PasswordResetFailed)}") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			return new BaseResult<string> { IsSuccess = true, Message = _localization.Get(ValidationError.AuthError.PasswordChanged) };
		}
		public async Task<BaseResult<string>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
		{
			var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

			if (user is  null)
			{
				return new BaseResult<string>($"{_localization.Get(ValidationError.AuthError.InvalidEmail)}:{forgotPasswordDto.Email}") { StatusCode = (int)HttpStatusCode.BadRequest };
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

			return new BaseResult<string> { IsSuccess = true, Message = _localization.Get(ValidationError.AuthError.CheckYourEmail) };
		}
		public async Task<BaseResult<TokenDto>> GoogleSigninAsync(string token)
		{
			try
			{
				var payload = await ValidateGoogleTokenAsync(token);

				var user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == payload.Data.Subject);

				if (user is null)
				{
					user = new AppUser
					{
						UserName = payload.Data.Subject,
						Email = payload.Data.Email,
					};

					var isfirstuser = !_userManager.Users.Any();

					var result = await _userManager.CreateAsync(user);

					if (!result.Succeeded)
					{
						throw new CustomException($"{ValidationError.AuthError.FailedToCreateUser}") { StatusCode = (int)HttpStatusCode.BadRequest };
					}
				}

				var tokens = await _tokenService.GenerateToken(user);

				var MakeToken = new TokenDto
				{
					Token = tokens,
					TokenType = "Bearer",
				};

				return new BaseResult<TokenDto> { Data = MakeToken };
			}
			catch (CustomException ex)
			{

				throw new CustomException($"Google Authentcation Failed{ex.Message}");
			}
		}

		public async Task<BaseResult<GoogleJsonWebSignature.Payload>> ValidateGoogleTokenAsync(string token)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings
			{
				Audience = new[] { _configuration["Authentication:Google:ClientId"] }
			};

			var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

			if (payload == null || string.IsNullOrEmpty(payload.Email))
			{
				throw new CustomException("Invalid Token") { StatusCode = (int)HttpStatusCode.Unauthorized };
			}

			return new BaseResult<GoogleJsonWebSignature.Payload> { Data = payload };
		}
	}
}
