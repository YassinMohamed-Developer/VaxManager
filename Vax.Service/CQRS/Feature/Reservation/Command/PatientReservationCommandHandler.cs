using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Data.Entity;
using Vax.Data.Enums;
using Vax.Repository.Interface;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Helper;
using Vax.Data.Entity;

namespace Vax.Service.CQRS.Feature.Reservation.Command
{

	//Need to refactor this code and make it more clean and readable and add more comments to explain the code
	public record PatientReservationCommand(ReservationRequestDto reservationRequestDto, string appuserid) : IRequest<BaseResult<string>>;
	public class PatientReservationCommandHandler : IRequestHandler<PatientReservationCommand, BaseResult<string>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public PatientReservationCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<string>> Handle(PatientReservationCommand request, CancellationToken cancellationToken)
		{
			var patient = await _unitOfWork.Patients.FindAsync(p => p.AppUserId == request.appuserid);

			if(patient == null)
			{
				throw new CustomException("Patient not found") { StatusCode = (int)HttpStatusCode.NotFound};
			}
			var vaccinecenter = await _unitOfWork.VaccinesCenter.FindAsync(vc => vc.Name == request.reservationRequestDto.VaccineName);

			if(vaccinecenter == null)
			{
				throw new CustomException("Vaccine center not found") { StatusCode = (int)HttpStatusCode.NotFound };
			}

			var vaccine = await _unitOfWork.Vaccines.FindAsync(v => v.Name == request.reservationRequestDto.VaccineName);

			if(vaccine == null)
			{
				throw new CustomException("Vaccine Not Found") { StatusCode = (int)HttpStatusCode.NotFound };
			}

			var reservation = await _unitOfWork.Reservations.FindAsync(p => p.PatientId == patient.Id
			&& p.VaccineId == vaccine.Id);

			if (reservation is null && request.reservationRequestDto.DoseNumber.Equals(DoseNumber.First) || (reservation is not null
	 && request.reservationRequestDto.DoseNumber.Equals(DoseNumber.First) && reservation.VaccineId != vaccine.Id))
			{
				reservation = new Data.Entity.Reservation();

				reservation.PatientId = patient.Id;
				reservation.DoseNumber = request.reservationRequestDto.DoseNumber;
				reservation.ReservationStatus = ReservationStatus.Pending;
				reservation.VaccineId = vaccine.Id;
				reservation.VaccineCenterId = vaccinecenter.Id;
				await _unitOfWork.Reservations.AddAsync(reservation);
				_unitOfWork.Complete();

				return new BaseResult<string> { IsSuccess = true, Message = "Patient Reserve Successfully " };
			}
			if (reservation is null && request.reservationRequestDto.Equals(DoseNumber.Second) || reservation is not null
				&& reservation.VaccineId != vaccine.Id && request.reservationRequestDto.DoseNumber.Equals(DoseNumber.Second))
			{
				throw new CustomException("You Can Not Reserve Second Dose Before First One") { StatusCode = (int)HttpStatusCode.BadRequest };

			}

			if (reservation is not null && reservation.DoseNumber.Equals(DoseNumber.First)
				== request.reservationRequestDto.DoseNumber.Equals(DoseNumber.First) && vaccine.Id == reservation.VaccineId)
			{
				throw new CustomException("You Already Take The First Dose ") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			if (reservation is not null && reservation.DoseNumber.Equals(DoseNumber.First)
				&& request.reservationRequestDto.DoseNumber.Equals(DoseNumber.Second))
			{
				if (request.reservationRequestDto.ReservationDate.Day - reservation.ReservationDate.Day < vaccine.TimeGapBetweenDoses)
				{
					throw new CustomException($"You Must Take Second Dose After {vaccine.TimeGapBetweenDoses} Days") { StatusCode = (int)HttpStatusCode.BadRequest };
				}
				_mapper.Map(request.reservationRequestDto, reservation);
				_unitOfWork.Complete();

				return new BaseResult<string> { IsSuccess = true, Message = "You Take The Second Dose" };
			}
			else
			{
				throw new CustomException("Can Not Reserve Second Dose Before First One must be Accepted") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

		}
	}
}
