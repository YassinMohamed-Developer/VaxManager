using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.RequestDto
{
	public class ResetPasswordDto
	{
		[Required]
		[RegularExpression("^[A-Z][A-Za-z\\d@$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-]{5,}$")]
		public string Password { get; set; }


		[Required]
		[Compare("Password",ErrorMessage ="Password doesn't Match The ConfirmPassword")]
		public string ConfirmPassword { get; set; }

		public string? Email {  get; set; }

		public string? token {  get; set; }
	}
}
