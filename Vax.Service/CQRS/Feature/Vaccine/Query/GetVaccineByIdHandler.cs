using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Repository.Interface;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Vaccine.Query
{
	public record GetVaccineByIdQuery(int Id) : IRequest<BaseResult<VaccineResponseDto>>;
	public class GetVaccineByIdHandler : IRequestHandler<GetVaccineByIdQuery, BaseResult<VaccineResponseDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetVaccineByIdHandler(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<VaccineResponseDto>> Handle(GetVaccineByIdQuery request, CancellationToken cancellationToken)
		{
			var vaccine = await _unitOfWork.Vaccines.FindAsync(v => v.Id == request.Id, includes: ["VaccineCenter"]);

			if(vaccine == null)
			{
				throw new CustomException("Vaccine Not Found") { StatusCode = (int)HttpStatusCode.NotFound };
			}

			var vaccinemap = _mapper.Map<VaccineResponseDto>(vaccine);

			return new BaseResult<VaccineResponseDto> { Data = vaccinemap, IsSuccess = true };
		}

	}
}
