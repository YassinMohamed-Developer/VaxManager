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
		public string FullName { get; set; }
		public string Email { get; set; }


		public AppUser AppUser { get; set; }

		public string AppUserId {  get; set; }
	}
}
