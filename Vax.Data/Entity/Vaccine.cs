using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Data.Entity
{
	public class Vaccine
	{

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		public int QuantityAvalible { get; set; }

		public string Precautions { get; set; }

		public int TimeGapBetweenDoses { get; set; }


		public VaccineCenter VaccineCenter { get; set; }

		public int? VaccineCenterId {  get; set; }


		public ICollection<Reservation> Reservations { get; set; } = [];

	}
}
