using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace service1.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClient _httpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            this._httpClient = new HttpClient();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> GetWeatherData()
        {
            var url = "http://localhost:4011/WeatherForecast";

            var accessToken = HttpContext.GetTokenAsync("Bearer", "access_token").Result ?? "";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var httpResponse = _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url), CancellationToken.None).Result;

            List<WeatherForecast> vm = new List<WeatherForecast>();
            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonString = httpResponse.Content.ReadAsStringAsync().Result;
                vm = JsonConvert.DeserializeObject<List<WeatherForecast>>(jsonString);
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Summary1 = vm[index-1].Summary

            })
            .ToArray();
        }
    }
}