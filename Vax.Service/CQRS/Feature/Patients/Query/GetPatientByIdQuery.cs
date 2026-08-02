using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Patients.Query
{
	public record GetPatientByIdQuery(int Id) : IRequest<BaseResult<PatientResponseDto>>;
}
