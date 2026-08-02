using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Vax.Data.Context;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Interface;

namespace Vax.Service.BackGroundJob
{
	public class DailyEmailMessageJob
	{
		private readonly Vaxdbcontext _vaxdbcontext;
		private readonly ILogger<DailyEmailMessageJob> _logger;
		private readonly IEmailService _emailService;

		public DailyEmailMessageJob(Vaxdbcontext vaxdbcontext,
			ILogger<DailyEmailMessageJob> logger,IEmailService emailService)
		{
			_vaxdbcontext = vaxdbcontext;
			_logger = logger;
			_emailService = emailService;
		}

		public async Task SendEmail()
		{
			var emails = await _vaxdbcontext.AppUsers.Select(x => x.Email).ToListAsync();

			if (emails.Any())
			{
				foreach (var email in emails)
				{
					var emailMessage = new EmailDto
					{
						Subject = "Daily Email",
						Body = "This is your daily email update.",
						To = email
					};
					_emailService.SendEmail(emailMessage);
				}

			}
			else
			{
				_logger.LogInformation("No emails found to send.");
			}
		}
	}
}
