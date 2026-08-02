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
	public class UpdateProfileCommandHandler : IRequestHandler<UpdatePatientProfileCommand, BaseResult<string>>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateProfileCommandHandler(UserManager<AppUser> userManager,IMapper mapper,IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<BaseResult<string>> Handle(UpdatePatientProfileCommand request, CancellationToken cancellationToken)
		{
			var userid = await _userManager.FindByIdAsync(request.appuserid);
			if(userid == null)
			{
				throw new CustomException("User is Invalid") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldpatient = await _unitOfWork.Patients.FindAsync(x => x.AppUserId == request.appuserid);

			if (oldpatient is null)
			{
				var patient = _mapper.Map<Patient>(request.PatientRequestDto);
				patient.AppUserId = request.appuserid;

				await _unitOfWork.Patients.AddAsync(patient);
				_unitOfWork.Complete();
			}
			else 
			{
				_mapper.Map(request.PatientRequestDto, oldpatient);
				_unitOfWork.Complete();
			}

			return new BaseResult<string> { Data = "Profile is Updated successfully",IsSuccess = true};
		}
	}
}
