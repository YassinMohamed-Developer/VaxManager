using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vax.Data.Entity;
using Vax.Repository.Interface;
using Vax.Service.Helper;

namespace Vax.Service.CQRS.Feature.Patients.Command
{
	public class CompleteProfileCommandHandler : IRequestHandler<CompleteProfileCommand, BaseResult<string>>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CompleteProfileCommandHandler(UserManager<AppUser> userManager,
			IUnitOfWork unitOfWork,IMapper mapper)
		{
			_userManager = userManager;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<BaseResult<string>> Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.AppUserId);

			if (user is null)
			{
				throw new CustomException("Invalid User") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldpatient = await _unitOfWork.Patients.FindAsync(x => x.AppUserId == request.AppUserId);
			if (oldpatient != null)
			{
				throw new CustomException("This Patient with Profile is Completed") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var patient = _mapper.Map<Patient>(request.PatientRequestDto);

			patient.AppUserId = request.AppUserId;

			await _unitOfWork.Patients.AddAsync(patient);
			_unitOfWork.Complete();

			return new BaseResult<string> { Data = patient.Id.ToString(), Message = "Profile Patient is Completed" };
		}
	}
}
