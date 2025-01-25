using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Data.Entity
{
	public class VaccineCenter
	{

		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Location {  get; set; }

		public string PhoneNumber {  get; set; }
		public AppUser AppUser { get; set; }

		public string? AppUserId { get; set; }

		public ICollection<Reservation> Reservations { get; set; } = [];
	}
}
