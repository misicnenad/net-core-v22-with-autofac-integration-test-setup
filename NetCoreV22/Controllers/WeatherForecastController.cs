using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NetCoreV22.Interfaces;
using NetCoreV22.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreV22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IRequestValidationService _validationService;

        public WeatherForecastController(IRequestValidationService validationService)
        {
            _validationService = validationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<WeatherForecast>> Get()
        {
            if (!_validationService.RequestCanBeProcessed())
            {
                return BadRequest();
            }

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
