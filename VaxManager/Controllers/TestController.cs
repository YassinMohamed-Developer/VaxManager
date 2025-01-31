using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace VaxManager.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi =true)]
	public class TestController : ControllerBase
	{
		private readonly ITestService _testService;

		public TestController(ITestService testService)
        {
			_testService = testService;
		}

		[Authorize(Roles = "Patient")]
		[HttpGet]
		public ActionResult<BaseResult<string>> GetbyId(int id)
		{

			var result = _testService.GetById(id);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
