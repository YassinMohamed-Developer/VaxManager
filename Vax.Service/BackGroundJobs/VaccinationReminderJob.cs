using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Vax.Data.Context;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Interface;

namespace Vax.Service.BackGroundJobs
{
	public class VaccinationReminderJob
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public VaccinationReminderJob(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		public async Task SendRemindersAsync()
		{
			using (var scope = _serviceScopeFactory.CreateScope())
			{

				var dp = scope.ServiceProvider.GetRequiredService<Vaxdbcontext>();
				var emailservice = scope.ServiceProvider.GetRequiredService<IEmailService>();

				var AllUsers = await dp.AppUsers.Where(x => x.Email != null).Select(x => x.Email).ToListAsync();

				foreach(var email in AllUsers)
				{
					emailservice.SendEmail(new EmailDto
					{
						Subject = "Vaccination Reminder",
						Body = $@"<html>
										<p>I hope this Email Find You Well it is your Welcome from VaxNova</p>" +
										"<p>if you Sick you can treat you</p>" +
							     "</html>",
						To = email
					});
				}

			}
		}
	}
}
