using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Enums;

namespace Vax.Service.DTOS.ResponseDto
{
	public class ReservationResponseDto
	{
		public int Id { get; set; }

		public string DoseNumber { get; set; }

		public string ReservationStatus { get; set; }

		public DateTime ReservationDate { get; set; }

		public string PatientName {  get; set; }

		public string VaccineName {  get; set; }

		public string VaccineCenterName {  get; set; }
	}
}
