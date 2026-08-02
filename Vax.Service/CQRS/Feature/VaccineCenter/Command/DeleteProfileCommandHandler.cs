using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Repository.Interface;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.VaccineCenter.Command
{
	public record DeleteProfileCommand(int VaccineId) : IRequest<BaseResult<string>>;
	public class DeleteProfileCommandHandler : IRequestHandler<DeleteProfileCommand, BaseResult<string>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public DeleteProfileCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<string>> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
		{
			var vaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.Id == request.VaccineId);
			if (vaccineCenter == null)
			{
				throw new CustomException("Vaccine Center not found") {StatusCode = (int)HttpStatusCode.NotFound };
			}

			 _unitOfWork.VaccinesCenter.Delete(vaccineCenter);
			 _unitOfWork.Complete();

			return new BaseResult<string>
			{
				Data = vaccineCenter.Id.ToString(),
				Message = "Vaccine Center deleted successfully",
			};
		}
	}
}
