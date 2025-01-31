using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.Interface
{
	public interface IVaccineCenterService
	{
		public Task<BaseResult<string>> CompleteProfileAsync(VaccineCenterRequestDto vaccineCenterRequest, string appuserid);

		public Task<BaseResult<string>> UpdateVaccineCenterProfile(VaccineCenterRequestDto vaccineCenterRequest, string appuserid);

		public Task<BaseResult<string>> DeleteProfile(int VaccineId);

		public Task<BaseResult<string>> CreateVaccine(VaccineRequestDto vaccine, string appuserid);

		public Task<BaseResult<IReadOnlyList<VaccineResponseDto>>> GetAllVaccines();


		public Task<BaseResult<VaccineResponseDto>> GetVaccineById(int Id);


		public Task<BaseResult<string>> UpdateVaccine(VaccineRequestDto vaccine, int vaccineId, string appuserid);


		public Task<BaseResult<string>> DeleteVaccine(int Id);

		public Task<BaseResult<string>> ApproveReservationById(int Id,string appuserid);

		public Task<BaseResult<string>> RejectReservationById(int Id, string appuserid);

		public Task<BaseResult<IReadOnlyList<PatientsWithVaccines>>> GetPatientsWithVaccines(string appuserid);
	}
}
