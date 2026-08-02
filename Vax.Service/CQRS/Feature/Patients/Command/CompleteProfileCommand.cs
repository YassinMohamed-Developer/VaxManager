using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Patients.Command
{
	public record CompleteProfileCommand(PatientRequestDto PatientRequestDto, string AppUserId) 
		: IRequest<BaseResult<string>>;
}
