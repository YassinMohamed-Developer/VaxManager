using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.Interface
{
	public interface IAdminService
	{
		public Task<BaseResult<IReadOnlyList<PatientResponseDto>>> GetAllPatients();

		public Task<BaseResult<IReadOnlyList<VaccineCenterResponseDto>>> GetAllVaccineCenter();

		public Task<BaseResult<AdminResponseDto>> GetAdminById(int id);


	}
}
