using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace VaxManager.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class HealthController : ControllerBase
	{
		private readonly HealthCheckService _healthCheckService;
		private readonly ILogger<HealthController> _logger;

		public HealthController(HealthCheckService healthCheckService,ILogger<HealthController> logger)
		{
			_healthCheckService = healthCheckService;
			_logger = logger;
		}
		[HttpGet]
		public async Task<IActionResult> CheckDatabaseHealth()
		{
			var report = await _healthCheckService.CheckHealthAsync();

			if(report.Status == HealthStatus.Healthy)
			{
				_logger.LogInformation("Database is Healthy");
				return Ok(new{ Database =  "Healthy" });
			}
			_logger.LogInformation("Database is UnHealthy");
			return StatusCode(305,new { Database = "UnHealthy" });
		}
	}
}
