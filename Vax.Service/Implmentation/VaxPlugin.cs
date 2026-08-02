using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Vax.Data.Context;
using Vax.Data.Entity;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
	public class VaxPlugin
	{
		private readonly Vaxdbcontext _vaxdbcontext;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailService _emailService;

		public VaxPlugin(Vaxdbcontext vaxdbcontext, UserManager<AppUser> userManager, IEmailService emailService)
		{
			_vaxdbcontext = vaxdbcontext;
			_userManager = userManager;
			_emailService = emailService;
		}
		[KernelFunction]
		[Description("This Function is responsible for That Every Day at 12:00 Send Email To the Users in the Database")]
		public async Task<BaseResult<string>> SendDailyEmail()
		{
			var users = await _userManager.Users.Select(x => x.Email).ToListAsync();
			foreach (var user in users)
			{
				_emailService.SendEmail(new EmailDto
				{
					Body = $@"<html>
									<p>This is your daily email from VaxManager. Stay healthy and informed!</p>
							</html>",
					Subject = "Daily Update from VaxManager",
					To = user,
				});
			}
			return new BaseResult<string>
			{
				Data = "Daily Emails Sent Successfully"
			};
		}

		[KernelFunction("GetVaccineCenterInfo")]
		[Description("This Function is responsible for retrieving information about vaccinecenters.")]
		public async Task<string> GetVaccineCenterInfo()
		{
			var vaccineCenters = await _vaxdbcontext.VaccineCenters.ToListAsync();

			if (vaccineCenters == null || vaccineCenters.Count == 0)
			{
				return "No Vaccine Centers Found";
			}
			var mappedVaccineCenters = vaccineCenters.Select(vc => new VaccineCenterResponseDto
			{
				Description = vc.Description,
				Id = vc.Id,
				Location = vc.Location,
				Name = vc.Name,
				PhoneNumber = vc.PhoneNumber,
			});

			return System.Text.Json.JsonSerializer.Serialize(mappedVaccineCenters);
		}
	}
}
