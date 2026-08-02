using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Patients.Command;
	public record UpdatePatientProfileCommand(PatientRequestDto PatientRequestDto, string appuserid) : IRequest<BaseResult<string>>;
