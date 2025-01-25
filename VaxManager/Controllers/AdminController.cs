using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace VaxManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]

        public async Task<ActionResult<BaseResult<PatientResponseDto>>> GetAllPatients()
        {
            var result = await _adminService.GetAllPatients();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<BaseResult<PatientResponseDto>>> GetAllVaccineCenters()
        {
            var result = await _adminService.GetAllVaccineCenter();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{adminid}")]
        public async Task<ActionResult<BaseResult<AdminResponseDto>>> GetAdminById(int adminid)
        {
            var result = await _adminService.GetAdminById(adminid);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
