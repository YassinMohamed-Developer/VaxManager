using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Enums;

namespace Vax.Service.DTOS.RequestDto
{
	public class ReservationRequestDto
	{
		[Required]
		public DoseNumber DoseNumber { get; set; }
		[Required]
		public string VaccineName {  get; set; }
		[Required]
		public string VaccineCenterName { get; set; }
		[Required]
		public DateTime ReservationDate { get; set; } = DateTime.Now;

	}
}
