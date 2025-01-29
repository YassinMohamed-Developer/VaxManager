using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.ResponseDto
{
	public class VaccineCenterWithVaccinesResponseDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Location { get; set; }

		public string Description { get; set; }

		public string PhoneNumber { get; set; }

		public ICollection<VaccineResponseDto> Vaccines { get; set; }
	}
}
