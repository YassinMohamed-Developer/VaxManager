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
	public class VaccineCenterController : ControllerBase
	{
		private readonly IVaccineCenterService _service;

		public VaccineCenterController(IVaccineCenterService service)
        {
			_service = service;
		}

		[Authorize(Roles = "VaccineCenter")]
		[HttpPost]

		public async Task<ActionResult<BaseResult<string>>> CreateVaccine([FromBody]VaccineRequestDto requestDto)
		{
			var AppUser = User.FindFirst("UserId").Value;

			var result = await _service.CreateVaccine(requestDto,AppUser);

			if(!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
		[Authorize(Roles = "VaccineCenter")]
		[HttpPut("{vaccineId}")]
		public async Task<ActionResult<BaseResult<string>>> UpdateVaccine([FromBody]VaccineRequestDto requestDto,int vaccineId)
		{
			var AppUser = User.FindFirst("UserId").Value;


			var result = await _service.UpdateVaccine(requestDto, vaccineId, AppUser);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
		[Authorize(Roles = "VaccineCenter")]
		[HttpDelete("{Vaccineid}")]

		public async Task<ActionResult<BaseResult<string>>> DeleteVaccine(int Vaccineid)
		{
			var result = await _service.DeleteVaccine(Vaccineid);
			if(!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<BaseResult<IReadOnlyList<VaccineResponseDto>>>> GetAllVaccine()
		{
			var result = await _service.GetAllVaccines();
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
		[HttpGet]
		public async Task<ActionResult<BaseResult<VaccineResponseDto>>> GetVaccineById(int vaccineId)
		{
			var result = await _service.GetVaccineById(vaccineId);
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[Authorize(Roles = "VaccineCenter")]
		[HttpPost]
		public async Task<ActionResult<BaseResult<string>>> CompleteProfileAsync(VaccineCenterRequestDto vaccineCenterRequest)
		{
			var AppUser = User.FindFirst("UserId").Value;

			var result = await _service.CompleteProfileAsync(vaccineCenterRequest, AppUser);
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}


		[Authorize(Roles = "VaccineCenter")]
		[HttpPut]

		public async Task<ActionResult<BaseResult<string>>> UpdateVaccineCenterProfile(VaccineCenterRequestDto vaccineCenterRequest)
		{
			var AppUser = User.FindFirst("UserId").Value;

			var result = await _service.UpdateVaccineCenterProfile(vaccineCenterRequest,AppUser);
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[Authorize(Roles = "VaccineCenter")]
		[HttpDelete("{VaccineId}")]
		public async Task<ActionResult<BaseResult<string>>> DeleteProfile(int VaccineId)
		{
			var result = await _service.DeleteProfile(VaccineId);
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
