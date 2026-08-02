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
	public class GetAllReservationQueryHandler : IRequestHandler<GetAllReservationQuery, BaseResult<IReadOnlyList<ReservationResponseDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllReservationQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<IReadOnlyList<ReservationResponseDto>>> Handle(GetAllReservationQuery request, CancellationToken cancellationToken)
		{
			var patientResrvation = await _unitOfWork.Reservations.FindAllAsync(x => true, includes:
				["Patient", "Vaccine", "VaccineCenter"]);

			if(patientResrvation == null)
			{
				throw new CustomException("No reservation found") { StatusCode = (int)HttpStatusCode.BadRequest};
			}

			var patientResrvationMap = _mapper.Map<IReadOnlyList<ReservationResponseDto>>(patientResrvation);

			return new BaseResult<IReadOnlyList<ReservationResponseDto>>
			{
				IsSuccess = true,
				Message = "Reservations retrieved successfully",
				Data = patientResrvationMap
			};
		}
	}
}
