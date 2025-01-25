using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Repository.Interface;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
	public class PatientService : IPatientService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;

		public PatientService(IUnitOfWork unitOfWork,IMapper mapper,UserManager<AppUser> userManager)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userManager = userManager;
		}
        public async Task<BaseResult<string>> CompleteProfileAsync(PatientRequestDto patientRequestDto, string appuserid)
		{
			var user = await _userManager.FindByIdAsync(appuserid);

			if(user == null)
			{
				throw new CustomException("InValid User") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldPatient = await _unitOfWork.Patients.FindAsync(x => x.AppUserId == appuserid);

			if(oldPatient != null)
			{
				throw new CustomException("This Patient Is Profile is Complete") { StatusCode = (int) HttpStatusCode.BadRequest };
			}

			var patient = _mapper.Map<Patient>(patientRequestDto);

			patient.AppUserId = appuserid;

			await _unitOfWork.Patients.AddAsync(patient);
			_unitOfWork.Complete();
			return new BaseResult<string> { Data = patient.Id.ToString(), Message = "Profile Patient is Completed" };
		}

		public async Task<BaseResult<string>> DeleteProfile(int PatientId)
		{
			var Patient = await _unitOfWork.Patients.GetByIdAsync(PatientId);
			if(Patient == null)
			{
				throw new CustomException("Patient is not Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			_unitOfWork.Patients.Delete(Patient);
			_unitOfWork.Complete();

			return new BaseResult<string> { Data = Patient.Id.ToString(), Message = "This Patient is Deleted Successfully" };
		}

		public async Task<BaseResult<PatientResponseDto>> GetPatientAsync(int Id)
		{
			var Patient = await _unitOfWork.Patients.GetByIdAsync(Id);
			if(Patient == null)
			{
				throw new CustomException("No Patient is Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Patientmap = _mapper.Map<PatientResponseDto>(Patient);

			return new BaseResult<PatientResponseDto> { Data = Patientmap, Message = "Data Retrieve Successfully " };
		}

		public async Task<BaseResult<string>> UpdatePatientProfile(PatientRequestDto patientRequestDto, string appuserid)
		{
			var user = await _userManager.FindByIdAsync(appuserid);

			if(user == null)
			{
				throw new CustomException("InValid User") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldPatient = await _unitOfWork.Patients.FindAsync(x => x.AppUserId == appuserid);

			if(oldPatient == null)
			{
				var patient = _mapper.Map<Patient>(patientRequestDto);

				patient.AppUserId = appuserid;

				await _unitOfWork.Patients.AddAsync(patient);

				_unitOfWork.Complete();
			}
			else
			{
				_mapper.Map(patientRequestDto,oldPatient);
				_unitOfWork.Complete();
			}

			return new BaseResult<string> { IsSuccess = true, Message = "Data Is Updated Successfully " };
		}
	}
}
