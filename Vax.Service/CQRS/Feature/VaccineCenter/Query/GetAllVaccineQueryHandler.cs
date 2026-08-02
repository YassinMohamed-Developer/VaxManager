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
	public record GetAllVaccineQuery() : IRequest<BaseResult<IReadOnlyList<VaccineResponseDto>>>;
	public class GetAllVaccineQueryHandler : IRequestHandler<GetAllVaccineQuery, BaseResult<IReadOnlyList<VaccineResponseDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllVaccineQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<IReadOnlyList<VaccineResponseDto>>> Handle(GetAllVaccineQuery request, CancellationToken cancellationToken)
		{
			var vaccines = await _unitOfWork.Vaccines.GetAllAsync();

			if(vaccines == null)
			{
				throw new CustomException("No Vaccines Found") { StatusCode = (int)HttpStatusCode.NotFound };
			}

			var vaccineMap = _mapper.Map<IReadOnlyList<VaccineResponseDto>>(vaccines);

			return new BaseResult<IReadOnlyList<VaccineResponseDto>>
			{
				Data = vaccineMap,
				IsSuccess = true,
			};
		}
	}
}
