using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Data.Enums;
using Vax.Repository.Interface;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;
using Vax.Service.Shared;

namespace Vax.Service.Implmentation
{
	public class VaccineCenterService : IVaccineCenterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;
		private readonly ILocalizationService _localization;

		public VaccineCenterService(IUnitOfWork unitOfWork,UserManager<AppUser> userManager,IMapper mapper,ILocalizationService localization)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_mapper = mapper;
			_localization = localization;
		}
		public async Task<BaseResult<string>> CompleteProfileAsync(VaccineCenterRequestDto vaccineCenterRequest, string appuserid)
		{
			var user = await _userManager.FindByIdAsync(appuserid);

			if(user == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.InvalidUser)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if(oldCenter != null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.ProfileAlreadyComplete)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var VaccineCenter = _mapper.Map<VaccineCenter>(vaccineCenterRequest);

			VaccineCenter.AppUserId = appuserid;

			await _unitOfWork.VaccinesCenter.AddAsync(VaccineCenter);
			_unitOfWork.Complete();
			 return new BaseResult<string> { Data = VaccineCenter.Id.ToString() ,Message = _localization.Get(ValidationError.VaccineCenterError.ProfileCompleted)};
		}

		public async Task<BaseResult<string>> CreateVaccine(VaccineRequestDto vaccine, string appuserid)
		{
			var vaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if (vaccineCenter == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineCenterNotFound)) { StatusCode = ((int)HttpStatusCode.BadRequest) };
			}
			 var Vaccine = _mapper.Map<Vaccine>(vaccine);

			Vaccine.VaccineCenter = vaccineCenter;
			await _unitOfWork.Vaccines.AddAsync(Vaccine);
			_unitOfWork.Complete();

			return new BaseResult<string> { IsSuccess = true, Message = _localization.Get(ValidationError.VaccineCenterError.VaccineCreated) };
		}

		public async Task<BaseResult<string>> DeleteProfile(int VaccineId)
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.GetByIdAsync(VaccineId);

			if(VaccineCenter == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineCenterNotFoundAlt)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			 _unitOfWork.VaccinesCenter.Delete(VaccineCenter);
			_unitOfWork.Complete();
			return new BaseResult<string> { Data = VaccineCenter.Id.ToString(), Message = _localization.Get(ValidationError.VaccineCenterError.ProfileDeleted) };
		}

		public async Task<BaseResult<string>> DeleteVaccine(int Id)
		{
			var Vaccine = await _unitOfWork.Vaccines.FindAsync(x => x.Id == Id);
			if(Vaccine == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			_unitOfWork.Vaccines.Delete(Vaccine);
			_unitOfWork.Complete();

			return new BaseResult<string> { Data = Vaccine.Id.ToString(), Message = _localization.Get(ValidationError.VaccineCenterError.DataDeletedSuccessfully) };
		}

		public async Task<BaseResult<IReadOnlyList<VaccineResponseDto>>> GetAllVaccines()
		{
			var Vaccine = await _unitOfWork.Vaccines.FindAllAsync(x => true, includes: ["VaccineCenter"]);

			if (Vaccine == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccinesNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}
			 var VaccineMap = _mapper.Map<IReadOnlyList<VaccineResponseDto>>(Vaccine);

			return new BaseResult<IReadOnlyList<VaccineResponseDto>> { Data = VaccineMap, Message = _localization.Get(ValidationError.VaccineCenterError.DataRetrieveSuccessfully) };
		}

		public async Task<BaseResult<VaccineResponseDto>> GetVaccineById(int Id)
		{
			var Vaccine = await _unitOfWork.Vaccines.FindAsync(x => x.Id == Id, includes:["VaccineCenter"]);
			if(Vaccine == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var VaccineMap = _mapper.Map<VaccineResponseDto>(Vaccine);

			return new BaseResult<VaccineResponseDto> { Data = VaccineMap, Message = _localization.Get(ValidationError.VaccineCenterError.DataRetrieveSuccessfullyAlt) };
		}

		public async Task<BaseResult<string>> UpdateVaccineCenterProfile(VaccineCenterRequestDto vaccineCenterRequest, string appuserid)
		{
			var user = await _userManager.FindByIdAsync(appuserid);

			if(user == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.InvalidUser)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);
			if (oldCenter == null)
			{
				var center = _mapper.Map<VaccineCenter>(vaccineCenterRequest);

				center.AppUserId = appuserid;
				await _unitOfWork.VaccinesCenter.AddAsync(center);
				_unitOfWork.Complete();
			}
			else
			{
				_mapper.Map(vaccineCenterRequest,oldCenter);
				_unitOfWork.Complete();
			}
			return new BaseResult<string> {IsSuccess = true ,Message = _localization.Get(ValidationError.VaccineCenterError.DataUpdatedSuccessfully) };
		}

		public async Task<BaseResult<string>> UpdateVaccine(VaccineRequestDto vaccine,int vaccineId, string appuserid)
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if(VaccineCenter == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineCenterNotFound)) { StatusCode = ((int)HttpStatusCode.BadRequest) };
			}

			var Vaccine = await _unitOfWork.Vaccines.GetByIdAsync(vaccineId);

			if (Vaccine == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}
			if(Vaccine.VaccineCenterId != VaccineCenter.Id)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.UnauthorizedAccess)) { StatusCode = (int)HttpStatusCode.BadRequest};
			}

			_mapper.Map(vaccine, Vaccine);
			_unitOfWork.Complete();

			return new BaseResult<string> { IsSuccess = true, Message = _localization.Get(ValidationError.VaccineCenterError.DataUpdatedSuccessfullyAlt) };
		}

		public async Task<BaseResult<string>> ApproveReservationById(int Id,string appuserid)
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if (VaccineCenter == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineCenterNotRegistered)) { StatusCode = ((int)HttpStatusCode.BadRequest) };
			}
			var Reservation = await _unitOfWork.Reservations.FindAsync(x => x.Id == Id, includes: ["Patient"]);


			if(Reservation == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.ReservationNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			if (Reservation is not null && Reservation.ReservationStatus.Equals(ReservationStatus.Pending)
				&& Reservation.VaccineCenterId == VaccineCenter.Id)
			{
				Reservation.ReservationStatus = ReservationStatus.Approved;
				_unitOfWork.Reservations.Update(Reservation);
				_unitOfWork.Complete();
				return new BaseResult<string> { Data = Reservation.Id.ToString(), Message = $"The Reservation of the Patient {Reservation.Patient.FirstName} is Accepted.."};
			}

			throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.ReservationNotAccepted)) { StatusCode = (int)HttpStatusCode.BadRequest };
		}

		public async Task<BaseResult<string>> RejectReservationById(int Id, string appuserid)
		{

			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if (VaccineCenter == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineCenterNotRegistered)) { StatusCode = ((int)HttpStatusCode.BadRequest) };
			}

			var Reservation = await _unitOfWork.Reservations.FindAsync(x => x.Id == Id, includes: ["Patient"]);

			if (Reservation == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.ReservationNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			if (Reservation is not null && Reservation.ReservationStatus.Equals(ReservationStatus.Pending)
				&& Reservation.VaccineCenterId == VaccineCenter.Id)
			{
				Reservation.ReservationStatus = ReservationStatus.Rejected;
				_unitOfWork.Reservations.Update(Reservation);
				_unitOfWork.Complete();
				return new BaseResult<string> { Data = Reservation.Id.ToString(), Message = $"The Reservation of the Patient {Reservation.Patient.FirstName} is Rejected.." };
			}

			throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.ReservationNotRejected)) 
			{ StatusCode = (int)HttpStatusCode.BadRequest };
		}

		public async Task<BaseResult<IReadOnlyList<PatientsWithVaccines>>> GetPatientsWithVaccines(string appuserid)
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if(VaccineCenter == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.VaccineCenterNotFoundAlt2)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Reservation = await _unitOfWork.Reservations.FindAllAsync(x => x.VaccineCenterId == VaccineCenter.Id,
				includes: ["Patient", "Vaccine", "VaccineCenter"]);

			if (Reservation == null)
			{
				throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.ReservationNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			if(Reservation is not null)
			{
				var Map = _mapper.Map<IReadOnlyList<PatientsWithVaccines>>(Reservation);
				return new BaseResult<IReadOnlyList<PatientsWithVaccines>> { Data = Map,Message = _localization.Get(ValidationError.VaccineCenterError.DataRetrieveSuccessfully) };
			}
			throw new CustomException(_localization.Get(ValidationError.VaccineCenterError.NoPatientsWithVaccines)) { StatusCode = (int)HttpStatusCode.BadRequest };
		}
	}
}
