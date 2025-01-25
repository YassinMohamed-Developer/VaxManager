using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.Interface
{
	public interface IPatientService
	{
		public Task<BaseResult<string>> CompleteProfileAsync(PatientRequestDto patientRequestDto,string appuserid);

		public Task<BaseResult<PatientResponseDto>> GetPatientAsync(int Id);

		public Task<BaseResult<string>> UpdatePatientProfile(PatientRequestDto patientRequestDto,string appuserid);

		public Task<BaseResult<string>> DeleteProfile(int PatientId);
	}
}
