using AppDemo.Models;
using AppDemo.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace AppDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetWeatherData()
        {
            string url = "http://localhost:4011/WeatherForecast/GetWeatherData";
            string accessToken = await GetAccessToken();

            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var httpResponse = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url), CancellationToken.None);

            List<WeatherForecastViewModel> vm = new List<WeatherForecastViewModel>();
            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                vm = JsonConvert.DeserializeObject<List<WeatherForecastViewModel>>(jsonString);
            }

            return View("Index", vm);
        }
        private async Task<string> GetAccessToken()
        {
            return await HttpContext.GetTokenAsync("access_token") ?? "";
        }
    }
}