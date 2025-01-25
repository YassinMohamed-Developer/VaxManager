using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.RequestDto
{
	public class LoginDto
	{
		[Required]
		[EmailAddress]
		public string Email {  get; set; }

		[Required]
		[RegularExpression("^[A-Z][A-Za-z\\d@$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-]{5,}$")]
		public string Password { get; set; }
	}
}
