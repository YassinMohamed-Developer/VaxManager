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
	public class PatientService : IPatientService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILocalizationService _localization;

		public PatientService(IUnitOfWork unitOfWork,IMapper mapper,UserManager<AppUser> userManager,ILocalizationService localization)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userManager = userManager;
			_localization = localization;
		}

		public async Task<BaseResult<string>> CancelReservation(int Id)
		{
			var Reservation = await _unitOfWork.Reservations.FindAsync(x => x.Id == Id);

			if(Reservation == null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.NoPatientFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			_unitOfWork.Reservations.Delete(Reservation);
			_unitOfWork.Complete();

			return new BaseResult<string> { IsSuccess = true,Message = _localization.Get(ValidationError.PatientError.ReservationCanceled) };
		}

		public async Task<BaseResult<string>> CompleteProfileAsync(PatientRequestDto patientRequestDto, string appuserid)
		{
			var user = await _userManager.FindByIdAsync(appuserid);

			if(user == null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.InvalidUser)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldPatient = await _unitOfWork.Patients.FindAsync(x => x.AppUserId == appuserid);

			if(oldPatient != null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.ProfileAlreadyComplete)) { StatusCode = (int) HttpStatusCode.BadRequest };
			}

			var patient = _mapper.Map<Patient>(patientRequestDto);

			patient.AppUserId = appuserid;

			await _unitOfWork.Patients.AddAsync(patient);
			_unitOfWork.Complete();
			return new BaseResult<string> { Data = patient.Id.ToString(), Message = _localization.Get(ValidationError.PatientError.ProfileCompleted) };
		}

		public async Task<BaseResult<string>> DeleteProfile(int PatientId)
		{
			var Patient = await _unitOfWork.Patients.GetByIdAsync(PatientId);
			if(Patient == null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.PatientNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			_unitOfWork.Patients.Delete(Patient);
			_unitOfWork.Complete();

			return new BaseResult<string> { Data = Patient.Id.ToString(), Message = _localization.Get(ValidationError.PatientError.PatientDeletedSuccessfully) };
		}

		public async Task<BaseResult<IReadOnlyList<ReservationResponseDto>>> GetAllReservation()
		{
			var Reservations = await _unitOfWork.Reservations.FindAllAsync(x => true, includes: ["Patient", "Vaccine", "VaccineCenter"]);

			if (Reservations is null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.ReservationsNotFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var reservationMap = _mapper.Map<IReadOnlyList<ReservationResponseDto>>(Reservations);

			return new BaseResult<IReadOnlyList<ReservationResponseDto>> { Data = reservationMap,Message = _localization.Get(ValidationError.PatientError.DataRetrieveSuccessfully) };
		}

		public async Task<BaseResult<PatientResponseDto>> GetPatientAsync(int Id)
		{
			var Patient = await _unitOfWork.Patients.GetByIdAsync(Id);
			if(Patient == null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.NoPatientFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Patientmap = _mapper.Map<PatientResponseDto>(Patient);

			return new BaseResult<PatientResponseDto> { Data = Patientmap, Message = _localization.Get(ValidationError.PatientError.DataRetrieveSuccessfully) };
		}

		public async Task<BaseResult<ReservationResponseDto>> GetReservationById(int Id)
		{
			var Reservation = await _unitOfWork.Reservations.FindAsync(x => x.Id == Id, includes: ["Patient", "Vaccine", "VaccineCenter"]);

			if(Reservation == null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.NoPatientFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Map = _mapper.Map<ReservationResponseDto>(Reservation);

			return new BaseResult<ReservationResponseDto> { Data = Map, Message = _localization.Get(ValidationError.PatientError.DataRetrieveSuccessfully) };
		}

		public async Task<BaseResult<VaccineCenterWithVaccinesResponseDto>> GetVaccineCenterWithVaccines(int VaccineCenterId)
		{
			var vaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(X => X.Id == VaccineCenterId, includes: ["Vaccines"]);

			if(vaccineCenter == null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.NoVaccineCenterFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Map = _mapper.Map<VaccineCenterWithVaccinesResponseDto>(vaccineCenter);

			return new BaseResult<VaccineCenterWithVaccinesResponseDto> { Data = Map,Message = _localization.Get(ValidationError.PatientError.DataRetrieveSuccessfully) };
		}

		public async Task<BaseResult<string>> PatientReservation(ReservationRequestDto reservationRequestDto, string appuserid)
		{
			var Patient = await _unitOfWork.Patients.FindAsync(x => x.AppUserId == appuserid);

			if (Patient is null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.NoPatientFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Vaccine = await _unitOfWork.Vaccines.FindAsync(x => x.Name == reservationRequestDto.VaccineName);

			if(Vaccine == null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.NoVaccineFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}
			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.Name == reservationRequestDto.VaccineCenterName);

			if(VaccineCenter is null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.NoVaccineCenterFound)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var reservation = await _unitOfWork.Reservations.FindAsync(x => x.PatientId == Patient.Id
			&& x.VaccineId == Vaccine.Id);


			///if you don't have reservation and need to reserve the FirstDose ====> then can Reservation
			///if have reservation in my database and need to reserve the first dose but in new Vaccine(new medicine) => Can Reservation 
			////if have reserve the First Dose and need to reserve the first Dose again in the Same Vaccine then not Reserve
			if (reservation is null && reservationRequestDto.DoseNumber.Equals(DoseNumber.First) || (reservation is not null 
				 && reservationRequestDto.DoseNumber.Equals(DoseNumber.First)&& reservation.VaccineId != Vaccine.Id))
			{
				reservation = new Reservation();

				reservation.PatientId = Patient.Id;
				reservation.DoseNumber = reservationRequestDto.DoseNumber;
				reservation.ReservationStatus = ReservationStatus.Pending;
				reservation.VaccineId = Vaccine.Id;
				reservation.VaccineCenterId = VaccineCenter.Id;
				await _unitOfWork.Reservations.AddAsync(reservation);
				_unitOfWork.Complete();

				return new BaseResult<string> { IsSuccess = true, Message = _localization.Get(ValidationError.PatientError.PatientReservedSuccessfully) };
			}

			if (reservation is null && reservationRequestDto.Equals(DoseNumber.Second) || reservation is not null
				&& reservation.VaccineId != Vaccine.Id && reservationRequestDto.DoseNumber.Equals(DoseNumber.Second))
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.CannotReserveSecondDoseFirst)) { StatusCode= (int)HttpStatusCode.BadRequest };

			}

			if(reservation is not null && reservation.DoseNumber.Equals(DoseNumber.First)
				== reservationRequestDto.DoseNumber.Equals(DoseNumber.First) && Vaccine.Id == reservation.VaccineId)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.AlreadyTakenFirstDose)) { StatusCode =  (int)HttpStatusCode.BadRequest };
			}

			if (reservation is not null && reservation.DoseNumber.Equals(DoseNumber.First)
				&& reservationRequestDto.DoseNumber.Equals(DoseNumber.Second))
			{
				if (reservationRequestDto.ReservationDate.Day - reservation.ReservationDate.Day < Vaccine.TimeGapBetweenDoses)
				{
					throw new CustomException(string.Format(_localization.Get(ValidationError.PatientError.MustWaitBetweenDoses), Vaccine.TimeGapBetweenDoses)) { StatusCode = (int)HttpStatusCode.BadRequest };
				}
				_mapper.Map(reservationRequestDto,reservation);
				_unitOfWork.Complete();

				return new BaseResult<string> { IsSuccess = true, Message = _localization.Get(ValidationError.PatientError.SecondDoseTaken) };
			}
			else
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.CannotReserveSecondDoseNotApproved)) { StatusCode = (int)HttpStatusCode.BadRequest };
			}
		}

		public async Task<BaseResult<string>> UpdatePatientProfile(PatientRequestDto patientRequestDto, string appuserid)
		{
			var user = await _userManager.FindByIdAsync(appuserid);

			if(user == null)
			{
				throw new CustomException(_localization.Get(ValidationError.PatientError.InvalidUser)) { StatusCode = (int)HttpStatusCode.BadRequest };
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

			return new BaseResult<string> { IsSuccess = true, Message = _localization.Get(ValidationError.PatientError.DataUpdatedSuccessfully) };
		}
	}
}
