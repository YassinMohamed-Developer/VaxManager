using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Repository.Interface;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Reservation.Command
{
	public record CancelReservationCommand(int ReservationId) : IRequest<BaseResult<string>>;
	public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, BaseResult<string>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CancelReservationCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<BaseResult<string>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
		{
			var reservation = await _unitOfWork.Reservations.FindAsync(x => x.Id == request.ReservationId);

			if(reservation == null)
			{
				throw new CustomException("This Reservation is Not Found to Be Canceled") { StatusCode = (int)HttpStatusCode.NotFound };

			}

			_unitOfWork.Reservations.Delete(reservation);
			_unitOfWork.Complete();

			return new BaseResult<string>
			{
				IsSuccess = true,
				Message = "Reservation Canceled Successfully",
				Data = reservation.Id.ToString(),
			};

		}
	}
}
