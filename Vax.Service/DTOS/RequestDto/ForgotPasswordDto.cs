using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.RequestDto
{
	public class ForgotPasswordDto
	{
		[Required]
		[EmailAddress]
		public string Email {  get; set; }

		//[Required]
		//public string ClientUrl { get; set; }
	}
}
