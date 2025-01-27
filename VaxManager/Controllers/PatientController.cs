using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		private readonly IPatientService _patientService;

		public PatientController(IPatientService patientService)
        {
			_patientService = patientService;
		}

		[Authorize(Roles = "Patient")]
		[HttpGet("{PatientId}")]
		public async Task<ActionResult<BaseResult<PatientResponseDto>>> GetPatient(int PatientId)
		{
			var result = await _patientService.GetPatientAsync(PatientId);
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

			var result = await _patientService.CompleteProfileAsync(input, UserId);

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

			var result = await _patientService.UpdatePatientProfile(input, UserId);

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
			var result = await _patientService.DeleteProfile(PatientId);

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

			var result = await _patientService.PatientReservation(requestDto,AppUser);

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
			var Result = await _patientService.GetAllReservation();

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
			var Result = await _patientService.GetReservationById(ReserveId);

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
			var Result = await _patientService.CancelReservation(ReserveId);

			if (!Result.IsSuccess)
			{
				return BadRequest(Result);
			}
			return Ok(Result);
		}
	}
}
