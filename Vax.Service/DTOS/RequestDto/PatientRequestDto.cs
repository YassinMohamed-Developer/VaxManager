using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.RequestDto
{
	public class PatientRequestDto
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Location { get; set; }

		public string City { get; set; }
	}
}
