using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Repository.Interface;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.VaccineCenter.Query
{

	public record GetVaccineCenterWithVaccinesQuery(int VaccineCenterId) : IRequest<BaseResult<VaccineCenterWithVaccinesResponseDto>>;
	public class GetVaccineCenterWithVaccinesQueryHandler : IRequestHandler<GetVaccineCenterWithVaccinesQuery, BaseResult<VaccineCenterWithVaccinesResponseDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetVaccineCenterWithVaccinesQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<VaccineCenterWithVaccinesResponseDto>> Handle(GetVaccineCenterWithVaccinesQuery request, CancellationToken cancellationToken)
		{
			var vaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(v => v.Id == request.VaccineCenterId,
				includes: ["Vaccines"]);

			if (vaccineCenter == null)
			{
				throw new CustomException("No Vaccine Center Not Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Map = _mapper.Map<VaccineCenterWithVaccinesResponseDto>(vaccineCenter);

			return new BaseResult<VaccineCenterWithVaccinesResponseDto> { Data = Map, Message = "Data Retrieve Successfully" };
		}
	}
}
