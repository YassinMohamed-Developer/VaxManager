using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Interface;
using Microsoft.Extensions.Options;
using Vax.Service.Helper;
using MimeKit;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Security;

namespace Vax.Service.Implmentation
{
	public class EmailService : IEmailService
	{
		private MailSettingsOptions _options;

		public EmailService(IOptions<MailSettingsOptions> options)
        {
			_options = options.Value;
		}
        public void SendEmail(EmailDto emailDto)
		{

			var mail = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_options.Email),
				Subject = emailDto.Subject,
			};

			mail.To.Add(MailboxAddress.Parse(emailDto.To));
			mail.From.Add(new MailboxAddress(_options.DisplayName,_options.Email));

			var builder = new BodyBuilder();
			builder.TextBody = emailDto.Body;

			mail.Body = builder.ToMessageBody();

			using var smtp = new SmtpClient();

			smtp.Connect(_options.Host, _options.Port,SecureSocketOptions.StartTls);

			smtp.Authenticate(_options.Email, _options.Password);

			smtp.Send(mail);

			smtp.Disconnect(true);
		}
	}
}
