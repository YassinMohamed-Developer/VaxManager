using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Reservation.Query
{
	public record GetAllReservationQuery() : IRequest<BaseResult<IReadOnlyList<ReservationResponseDto>>>;
}
