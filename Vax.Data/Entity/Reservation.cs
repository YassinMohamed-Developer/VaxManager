using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Enums;

namespace Vax.Data.Entity
{
	public class Reservation
	{
		public int Id { get; set; }


		public DoseNumber DoseNumber { get; set; }

		public ReservationStatus ReservationStatus { get; set; }

		public DateTime ReservationDate { get; set; } = DateTime.Now;


		public Patient Patient { get; set; }

		public int? PatientId {  get; set; }

		public Vaccine Vaccine {  get; set; }

		public int? VaccineId { get;set; }

		public VaccineCenter VaccineCenter { get; set; } // Approve

		public int? VaccineCenterId {  get; set; }

	}
}
