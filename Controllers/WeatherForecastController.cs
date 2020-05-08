using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using json_tagger_api;

namespace json_tagger_api.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //var a = Directory.EnumerateFiles("/data");
            //var b = Summaries[0];
            using (var context = new TaggerDbContext(new DbContextOptions<TaggerDbContext>()))
            {
                var rng = new Random();
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    //Summary = Summaries[rng.Next(Summaries.Length)]
                    //Summary = Directory.EnumerateFiles("/data").ToArray()[0]
                    Summary = context.FileRecords.First().FilePath
                })
                .ToArray();
            }
        }
    }
}
