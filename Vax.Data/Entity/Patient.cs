using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Data.Entity
{
	public class Patient
	{

		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Location {  get; set; }

		public string City { get; set; }


		public AppUser AppUser { get; set; }

		public string? AppUserId { get; set; }


		public ICollection<Reservation> Reservations { get; set; } = [];
	}
}
