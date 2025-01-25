using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.ResponseDto
{
	public class VaccineResponseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		public int QuantityAvalible { get; set; }

		public string Precautions { get; set; }

		public int TimeGapBetweenDoses { get; set; }

		public string VaccineCenterName {  get; set; }
		public int? VaccineCenterId { get; set; }
	}
}
