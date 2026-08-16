using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Serilog;
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
using Vax.Service.Shared;

namespace Vax.Service.Implmentation
{
	public class AdminService : IAdminService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger<AdminService> _logger;
		private readonly ILocalizationService _localization;

		public AdminService(IUnitOfWork unitOfWork,IMapper mapper,ILogger<AdminService> logger,ILocalizationService localization)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
			_localization = localization;
		}
		public async Task<BaseResult<AdminResponseDto>> GetAdminById(int id)
		{
			var admin = await _unitOfWork.Admins.GetByIdAsync(id);
			if (admin == null)
			{
				throw new CustomException(_localization.Get(ValidationError.AdminError.AdminNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}
			var adminmap = _mapper.Map<AdminResponseDto>(admin);

			return new BaseResult<AdminResponseDto> { Data = adminmap,Message = _localization.Get(ValidationError.AdminError.DataRetrieveSuccessfully) };
		}

		public async Task<BaseResult<IReadOnlyList<PatientResponseDto>>> GetAllPatients()
		{
			var Patient = await _unitOfWork.Patients.GetAllAsync();

			if (Patient == null)
			{
				throw new CustomException(_localization.Get(ValidationError.AdminError.NoPatientsAdded)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var PatientMap = _mapper.Map<IReadOnlyList<PatientResponseDto>>(Patient);


			_logger.Log(LogLevel.Warning, "GetAllPatients method called in AdminService");
			return new BaseResult<IReadOnlyList<PatientResponseDto>> { Data = PatientMap, Message = _localization.Get(ValidationError.AdminError.DataRetrieveSuccessfully) };
		}

		public async Task<BaseResult<IReadOnlyList<VaccineCenterResponseDto>>> GetAllVaccineCenter()
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.DapperGetAllAsync();

			if (VaccineCenter == null)
			{
				throw new CustomException(_localization.Get(ValidationError.AdminError.NoVaccineCenterAdded)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var VaccineCenterMap = _mapper.Map<IReadOnlyList<VaccineCenterResponseDto>>(VaccineCenter);

			return new BaseResult<IReadOnlyList<VaccineCenterResponseDto>> { Data = VaccineCenterMap,Message = _localization.Get(ValidationError.AdminError.DataRetrieveSuccessfully) };
		}
	}
}
