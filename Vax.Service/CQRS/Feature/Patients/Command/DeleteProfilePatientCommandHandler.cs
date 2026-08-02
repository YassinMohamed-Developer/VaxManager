using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Repository.Interface;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Patients.Command
{
	public class DeleteProfilePatientCommandHandler : IRequestHandler<DeleteProfilePatientCommand, BaseResult<string>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteProfilePatientCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<BaseResult<string>> Handle(DeleteProfilePatientCommand request, CancellationToken cancellationToken)
		{

			var patient = await _unitOfWork.Patients.FindAsync(P => P.Id == request.PatientId);

			if(patient == null)
			{
				throw new CustomException("Patient is not Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			_unitOfWork.Patients.Delete(patient);
			_unitOfWork.Complete();

			return new BaseResult<string>
			{
				IsSuccess = true,
				Message = "Patient Profile Deleted Successfully",
				Data = patient.Id.ToString(),
			};
		}
	}
}
