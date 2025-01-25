using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Data.Entity
{
	public class AppUser : IdentityUser
	{
		public Patient Patient { get; set; }

		public VaccineCenter VaccineCenter { get; set; }

		public Admin Admin { get; set; }
	}
}
