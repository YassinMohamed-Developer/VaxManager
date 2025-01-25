using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.RequestDto
{
	public class VaccineRequestDto
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int QuantityAvalible { get; set; }

		[Required]
		public string Precautions { get; set; }

		[Required]
		public int TimeGapBetweenDoses { get; set; }
	}
}
