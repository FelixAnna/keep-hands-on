using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly WeatherClient client;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherClient client)
        {
            _logger = logger;
            this.client = client;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("{city}")]
        public async Task<WeatherForecast> GetForecastAsync(string city)
        {
            var forecast = await client.GetForecatAsync(city);
            return new WeatherForecast()
            {
                Date = DateTimeOffset.FromUnixTimeSeconds(forecast.dt).DateTime,
                Summary = forecast.weathers[0].description,
                TemperatureC = (int)forecast.main.temp
            };
        }
    }
}
