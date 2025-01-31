using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.ResponseDto
{
	public class PatientsWithVaccines
	{


		public int PatientId { get; set; }

		public string PatientName { get; set; }

		public string Location { get; set; }

		public string City { get; set; }


		public int VaccineId { get; set; }
		public string VaccineName { get; set; }

	}
}
