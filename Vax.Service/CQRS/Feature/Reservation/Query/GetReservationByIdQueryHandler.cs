using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Repository.Interface;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Reservation.Query
{

	public record GetReservationByIdQuery(int Id) : IRequest<BaseResult<ReservationResponseDto>>;
	public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, BaseResult<ReservationResponseDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetReservationByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<ReservationResponseDto>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
		{
			var patientReservation = await _unitOfWork.Reservations.FindAsync(r => r.Id == request.Id,
				includes: ["Patient", "Vaccine", "VaccineCenter"]);

			if(patientReservation == null)
			{
				throw new CustomException("Reservation not found") {StatusCode = (int)HttpStatusCode.NotFound};
			}

			var PatientReservationMapped = _mapper.Map<ReservationResponseDto>(patientReservation);

			return new BaseResult<ReservationResponseDto>
			{
				IsSuccess = true,
				Message = "Reservation retrieved successfully",
				Data = PatientReservationMapped
			};
		}
	}
}
