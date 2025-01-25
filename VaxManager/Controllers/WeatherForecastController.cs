using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vax.Service.Helper;
using Vax.Service.Implmentation;
using Vax.Service.Interface;

namespace VaxManager.Controllers
{
	[Route("[controller]/[action]")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)] // FOR Ignore These Controller for swaggerDecomentation
	public class WeatherForecastController : ControllerBase
	{
        private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly ITestService testService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}


	
	}
}
