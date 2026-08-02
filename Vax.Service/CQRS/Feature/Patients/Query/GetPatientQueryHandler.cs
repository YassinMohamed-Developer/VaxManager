using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Repository.Interface;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Patients.Query
{
	public class GetPatientQueryHandler : IRequestHandler<GetPatientByIdQuery, BaseResult<PatientResponseDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetPatientQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<PatientResponseDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
		{
			var patient = await _unitOfWork.Patients.GetByIdAsync(request.Id);

			if (patient == null)
			{
				throw new CustomException("No Patient is Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}
			var PatientMap = _mapper.Map<PatientResponseDto>(patient);

			return new BaseResult<PatientResponseDto> { Data = PatientMap, Message = "Data Retrieve Successfully " };
		}
	}
}
