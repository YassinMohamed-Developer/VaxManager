using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vax.Service.CQRS.Feature.Patients.Command;
using Vax.Service.CQRS.Feature.Patients.Query;
using Vax.Service.CQRS.Feature.Reservation.Command;
using Vax.Service.CQRS.Feature.Reservation.Query;
using Vax.Service.CQRS.Feature.VaccineCenter.Query;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace VaxManager.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private readonly IMediator _mediator;

		public PatientController(IMediator mediator)
        {
			_mediator = mediator;
		}

		//[Authorize(Roles = "Patient")]
		[HttpGet("{PatientId}")]
		public async Task<ActionResult<BaseResult<PatientResponseDto>>> GetPatient(int PatientId)
		{
			var result = await _mediator.Send(new GetPatientByIdQuery(PatientId));
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[Authorize(Roles = "Patient")]
		[HttpPost("")]

		public async Task<ActionResult<BaseResult<string>>> CompleteProfile([FromBody] PatientRequestDto input)
		{
			var UserId = User.FindFirst("UserId").Value;

			var result = await _mediator.Send(new CompleteProfileCommand(input, UserId));

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[Authorize(Roles = "Patient")]
		[HttpPut]
		public async Task<ActionResult<BaseResult<string>>> UpdateProfile([FromBody] PatientRequestDto input)
		{
			var UserId = User.FindFirst("UserId").Value;

			var result = await _mediator.Send(new UpdatePatientProfileCommand(input, UserId));

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
		[Authorize(Roles = "Patient")]
		[HttpDelete("{PatientId}")]
		public async Task<ActionResult<BaseResult<string>>> DeleteProfile(int PatientId)
		{
			var result = await _mediator.Send(new DeleteProfilePatientCommand(PatientId));

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[Authorize(Roles = "Patient")]
		[HttpPost]
		public async Task<ActionResult<BaseResult<string>>> ReservationVaccine(ReservationRequestDto requestDto)
		{
			var AppUser = User.FindFirst("UserId").Value;

			var result = await _mediator.Send(new PatientReservationCommand(requestDto, AppUser));

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[Authorize(Roles = "Patient")]
		[HttpGet("all")]
		public async Task<ActionResult<BaseResult<IReadOnlyList<ReservationResponseDto>>>> GetAllReservation()
		{
			var Result = await _mediator.Send(new GetAllReservationQuery());

			if (!Result.IsSuccess)
			{
				return BadRequest(Result);
			}
			return Ok(Result);
		}
		[Authorize(Roles = "Patient")]
		[HttpGet("{ReserveId}")]
		public async Task<ActionResult<BaseResult<ReservationResponseDto>>> GetReservationById(int ReserveId)
		{
			var Result = await _mediator.Send(new GetReservationByIdQuery(ReserveId));

			if (!Result.IsSuccess)
			{
				return BadRequest(Result);
			}
			return Ok(Result);
		}

		[Authorize(Roles = "Patient")]
		[HttpDelete("{ReserveId}")]
		public async Task<ActionResult<BaseResult<string>>> CancelReservation(int ReserveId)
		{
			var Result = await _mediator.Send(new CancelReservationCommand(ReserveId));

			if (!Result.IsSuccess)
			{
				return BadRequest(Result);
			}
			return Ok(Result);
		}
		[Authorize(Roles = "Patient")]
		[HttpGet("{VaccineCenterId}")]
		public async Task<ActionResult<BaseResult<VaccineCenterWithVaccinesResponseDto>>> GetVaccineCenterwithVaccines(int VaccineCenterId)
		{
			var Result = await _mediator.Send(new GetVaccineCenterWithVaccinesQuery(VaccineCenterId));

			if (!Result.IsSuccess)
			{
				return BadRequest(Result);
			}
			return Ok(Result);
		}
	}
}
