using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Data.Entity
{
	public class Admin
	{
		public int Id { get; set; }
		public string FullName { get; set; } = null!;
		public string Email { get; set; } = null!;


		public AppUser AppUser { get; set; } = null!;

		public string AppUserId { get; set; } = null!;
	}
}
