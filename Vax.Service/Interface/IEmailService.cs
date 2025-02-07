using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Service.DTOS.RequestDto;

namespace Vax.Service.Interface
{
	public interface IEmailService
	{
		public void SendEmail(EmailDto emailDto);
	}
}
