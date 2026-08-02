using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Patients.Command
{
	public record DeleteProfilePatientCommand(int PatientId) : IRequest<BaseResult<string>>;

}
