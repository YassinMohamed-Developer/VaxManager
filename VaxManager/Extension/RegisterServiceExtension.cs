using Microsoft.AspNetCore.Mvc;
using Vax.Data.Entity;
using Vax.Repository.Implmentation;
using Vax.Repository.Interface;
using Vax.Service.Helper;
using Vax.Service.Implmentation;
using Vax.Service.Interface;
using Vax.Service.Mapper;

namespace VaxManager.Extension
{
	public static class RegisterServiceExtension
	{
		public static void RegisterService(this IServiceCollection service)
		{
			service.AddScoped<IUnitOfWork, UnitOfWork>();
			service.AddScoped<ITestService, TestService>();
			service.AddScoped<ITokenService, TokenService>();
			service.AddScoped<IAuthService, AuthService>();
			service.AddScoped<IAdminService, AdminService>();
			service.AddScoped<IPatientService, PatientService>();
			service.AddScoped<IVaccineCenterService, VaccineCenterService>();
			service.AddScoped<IEmailService, EmailService>();
			service.AddScoped<ISmsService, SmsService>();

			service.AddAutoMapper(typeof(AdminProfile));
			service.AddAutoMapper(typeof(PatientProfile));
			service.AddAutoMapper(typeof(VaccineCenterProfile));
			service.AddAutoMapper(typeof(Vaccine));
			service.AddAutoMapper(typeof(ReservationProfile));

			service.Configure<ApiBehaviorOptions>(option =>
			{
				option.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var error = actionContext.ModelState.Where(p => p.Value?.Errors.Count > 0)
													.SelectMany(e => e.Value.Errors)
													.Select(m => m.ErrorMessage)
													.ToList();
					var response = new BaseResult<string>()
					{
						IsSuccess = false,
						Errors = error
					};

					return new BadRequestObjectResult(response);
				};
			});
		}
	}
}
