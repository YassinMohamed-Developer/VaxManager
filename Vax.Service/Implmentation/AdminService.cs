using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vax.Repository.Interface;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
	public class AdminService : IAdminService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public AdminService(IUnitOfWork unitOfWork,IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
        public async Task<BaseResult<AdminResponseDto>> GetAdminById(int id)
		{
			var admin = await _unitOfWork.Admins.GetByIdAsync(id);
			if (admin == null)
			{
				throw new CustomException("Admin Not Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}
			var adminmap = _mapper.Map<AdminResponseDto>(admin);

			return new BaseResult<AdminResponseDto> { Data = adminmap,Message = "Data Retrieve Successfully " };
		}

		public async Task<BaseResult<IReadOnlyList<PatientResponseDto>>> GetAllPatients()
		{
			var Patient = await _unitOfWork.Patients.GetAllAsync();

			if (Patient == null)
			{
				throw new CustomException("No Patients Added") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var PatientMap = _mapper.Map<IReadOnlyList<PatientResponseDto>>(Patient);

			return new BaseResult<IReadOnlyList<PatientResponseDto>> { Data = PatientMap, Message = "Data Retrieve Successfully " };
		}

		public async Task<BaseResult<IReadOnlyList<VaccineCenterResponseDto>>> GetAllVaccineCenter()
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.DapperGetAllAsync();

			if (VaccineCenter == null)
			{
				throw new CustomException("No Vaccine Center Is Added") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var VaccineCenterMap = _mapper.Map<IReadOnlyList<VaccineCenterResponseDto>>(VaccineCenter);

			return new BaseResult<IReadOnlyList<VaccineCenterResponseDto>> { Data = VaccineCenterMap,Message = "Data Retrieve Successfully " };
		}
	}
}
