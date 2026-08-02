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
		//Command 
		public Task<BaseResult<string>> CompleteProfileAsync(VaccineCenterRequestDto vaccineCenterRequest, string appuserid);

		//Command
		public Task<BaseResult<string>> UpdateVaccineCenterProfile(VaccineCenterRequestDto vaccineCenterRequest, string appuserid);

		//Command
		public Task<BaseResult<string>> CreateVaccine(VaccineRequestDto vaccine, string appuserid);

		//Command
		public Task<BaseResult<string>> UpdateVaccine(VaccineRequestDto vaccine, int vaccineId, string appuserid);
		//Command

		public Task<BaseResult<string>> DeleteVaccine(int Id);
		//Command
		public Task<BaseResult<string>> ApproveReservationById(int Id,string appuserid);
		//Command
		public Task<BaseResult<string>> RejectReservationById(int Id, string appuserid);
		//Query
		public Task<BaseResult<IReadOnlyList<PatientsWithVaccines>>> GetPatientsWithVaccines(string appuserid);
	}
}
