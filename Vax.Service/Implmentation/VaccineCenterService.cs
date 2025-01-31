using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Data.Enums;
using Vax.Repository.Interface;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
	public class VaccineCenterService : IVaccineCenterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;

		public VaccineCenterService(IUnitOfWork unitOfWork,UserManager<AppUser> userManager,IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_mapper = mapper;
		}
        public async Task<BaseResult<string>> CompleteProfileAsync(VaccineCenterRequestDto vaccineCenterRequest, string appuserid)
		{
			var user = await _userManager.FindByIdAsync(appuserid);

			if(user == null)
			{
				throw new CustomException("InValid User") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if(oldCenter != null)
			{
				throw new CustomException("This VaccineCenter Is Profile is Complete") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var VaccineCenter = _mapper.Map<VaccineCenter>(vaccineCenterRequest);

			VaccineCenter.AppUserId = appuserid;

			await _unitOfWork.VaccinesCenter.AddAsync(VaccineCenter);
			_unitOfWork.Complete();
			 return new BaseResult<string> { Data = VaccineCenter.Id.ToString() ,Message = "The Profile Is Completed"};
		}

		public async Task<BaseResult<string>> CreateVaccine(VaccineRequestDto vaccine, string appuserid)
		{
			var vaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if (vaccineCenter == null)
			{
				throw new CustomException("No VaccineCene Not Found") { StatusCode = ((int)HttpStatusCode.BadRequest) };
			}
			 var Vaccine = _mapper.Map<Vaccine>(vaccine);

			Vaccine.VaccineCenter = vaccineCenter;
			await _unitOfWork.Vaccines.AddAsync(Vaccine);
			_unitOfWork.Complete();

			return new BaseResult<string> { IsSuccess = true, Message = "Vaccine Is Created" };
		}

		public async Task<BaseResult<string>> DeleteProfile(int VaccineId)
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.GetByIdAsync(VaccineId);

			if(VaccineCenter == null)
			{
				throw new CustomException("No VaccineCenter is Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			 _unitOfWork.VaccinesCenter.Delete(VaccineCenter);
			_unitOfWork.Complete();
			return new BaseResult<string> { Data = VaccineCenter.Id.ToString(), Message = "Profile Is Deleted" };
		}

		public async Task<BaseResult<string>> DeleteVaccine(int Id)
		{
			var Vaccine = await _unitOfWork.Vaccines.FindAsync(x => x.Id == Id);
			if(Vaccine == null)
			{
				throw new CustomException("No Vaccine is Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			_unitOfWork.Vaccines.Delete(Vaccine);
			_unitOfWork.Complete();

			return new BaseResult<string> { Data = Vaccine.Id.ToString(), Message = "Data Is Deleted Successfully" };
		}

		public async Task<BaseResult<IReadOnlyList<VaccineResponseDto>>> GetAllVaccines()
		{
			var Vaccine = await _unitOfWork.Vaccines.FindAllAsync(x => true, includes: ["VaccineCenter"]);

			if (Vaccine == null)
			{
				throw new CustomException("No Vaccines is Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}
			 var VaccineMap = _mapper.Map<IReadOnlyList<VaccineResponseDto>>(Vaccine);

			return new BaseResult<IReadOnlyList<VaccineResponseDto>> { Data = VaccineMap, Message = "Data Is Retrieve Successfully" };
		}

		public async Task<BaseResult<VaccineResponseDto>> GetVaccineById(int Id)
		{
			var Vaccine = await _unitOfWork.Vaccines.FindAsync(x => x.Id == Id, includes:["VaccineCenter"]);
			if(Vaccine == null)
			{
				throw new CustomException("No Vaccine Is Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var VaccineMap = _mapper.Map<VaccineResponseDto>(Vaccine);

			return new BaseResult<VaccineResponseDto> { Data = VaccineMap, Message = "Data Is Retrieve Successfully!" };
		}

		public async Task<BaseResult<string>> UpdateVaccineCenterProfile(VaccineCenterRequestDto vaccineCenterRequest, string appuserid)
		{
			var user = await _userManager.FindByIdAsync(appuserid);

			if(user == null)
			{
				throw new CustomException("Invalid User") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var oldCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);
			if (oldCenter == null)
			{
				var center = _mapper.Map<VaccineCenter>(vaccineCenterRequest);

				center.AppUserId = appuserid;
				await _unitOfWork.VaccinesCenter.AddAsync(center);
				_unitOfWork.Complete();
			}
			else
			{
				_mapper.Map(vaccineCenterRequest,oldCenter);
				_unitOfWork.Complete();
			}
			return new BaseResult<string> {IsSuccess = true ,Message = "Data Is Updated Successfully " };
		}

		public async Task<BaseResult<string>> UpdateVaccine(VaccineRequestDto vaccine,int vaccineId, string appuserid)
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if(VaccineCenter == null)
			{
				throw new CustomException("No VaccineCene Not Found") { StatusCode = ((int)HttpStatusCode.BadRequest) };
			}

			var Vaccine = await _unitOfWork.Vaccines.GetByIdAsync(vaccineId);

			if (Vaccine == null)
			{
				throw new CustomException("No Vaccine Is Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}
			if(Vaccine.VaccineCenterId != VaccineCenter.Id)
			{
				throw new CustomException("Unauthorized Access") { StatusCode = (int)HttpStatusCode.BadRequest};
			}

			_mapper.Map(vaccine, Vaccine);
			_unitOfWork.Complete();

			return new BaseResult<string> { IsSuccess = true, Message = "Data Updated Successfully" };
		}

		public async Task<BaseResult<string>> ApproveReservationById(int Id,string appuserid)
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if (VaccineCenter == null)
			{
				throw new CustomException("This VaccineCenter Account Not Registered") { StatusCode = ((int)HttpStatusCode.BadRequest) };
			}
			var Reservation = await _unitOfWork.Reservations.FindAsync(x => x.Id == Id, includes: ["Patient"]);


			if(Reservation == null)
			{
				throw new CustomException("Reservation Not Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			if (Reservation is not null && Reservation.ReservationStatus.Equals(ReservationStatus.Pending)
				&& Reservation.VaccineCenterId == VaccineCenter.Id)
			{
				Reservation.ReservationStatus = ReservationStatus.Approved;
				_unitOfWork.Reservations.Update(Reservation);
				_unitOfWork.Complete();
				return new BaseResult<string> { Data = Reservation.Id.ToString(), Message = $"The Reservation of the Patient {Reservation.Patient.FirstName} is Accepted.."};
			}

			throw new CustomException("The Reservation Not Accepted Or This Vaccine Center Account Not Have Reservations") { StatusCode = (int)HttpStatusCode.BadRequest };
		}

		public async Task<BaseResult<string>> RejectReservationById(int Id, string appuserid)
		{

			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if (VaccineCenter == null)
			{
				throw new CustomException("This VaccineCenter Account Not Registered") { StatusCode = ((int)HttpStatusCode.BadRequest) };
			}

			var Reservation = await _unitOfWork.Reservations.FindAsync(x => x.Id == Id, includes: ["Patient"]);

			if (Reservation == null)
			{
				throw new CustomException("Reservation Not Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			if (Reservation is not null && Reservation.ReservationStatus.Equals(ReservationStatus.Pending)
				&& Reservation.VaccineCenterId == VaccineCenter.Id)
			{
				Reservation.ReservationStatus = ReservationStatus.Rejected;
				_unitOfWork.Reservations.Update(Reservation);
				_unitOfWork.Complete();
				return new BaseResult<string> { Data = Reservation.Id.ToString(), Message = $"The Reservation of the Patient {Reservation.Patient.FirstName} is Rejected.." };
			}

			throw new CustomException("The Reservation Not Rejected Because It Is Accepted. Or This Vaccine Center Account Not Have Reservations") 
			{ StatusCode = (int)HttpStatusCode.BadRequest };
		}

		public async Task<BaseResult<IReadOnlyList<PatientsWithVaccines>>> GetPatientsWithVaccines(string appuserid)
		{
			var VaccineCenter = await _unitOfWork.VaccinesCenter.FindAsync(x => x.AppUserId == appuserid);

			if(VaccineCenter == null)
			{
				throw new CustomException("Vaccine Center is Not Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Reservation = await _unitOfWork.Reservations.FindAllAsync(x => x.VaccineCenterId == VaccineCenter.Id,
				includes: ["Patient", "Vaccine", "VaccineCenter"]);

			if (Reservation == null)
			{
				throw new CustomException("Reservation Not Found") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			if(Reservation is not null)
			{
				var Map = _mapper.Map<IReadOnlyList<PatientsWithVaccines>>(Reservation);
				return new BaseResult<IReadOnlyList<PatientsWithVaccines>> { Data = Map,Message = "Data Retrieve Successfully.." };
			}
			throw new CustomException("No Patients With Vaccines in this VaccineCenter..") { StatusCode = (int)HttpStatusCode.BadRequest };
		}
	}
}
