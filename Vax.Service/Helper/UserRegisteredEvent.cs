using System;
using System.Collections.Generic;
using System.Text;

namespace Vax.Service.Helper
{
	public class UserRegisteredEvent
	{
		public string UserId { get; set; }

		public string? Email { get; set; }

		public string? FullName { get; set; }
	}
}
