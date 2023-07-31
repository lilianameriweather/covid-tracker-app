using COVIDTrackerApp.Models;
using COVIDTrackerApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace COVIDTrackerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICovidCaseService _covidCaseService;

        public HomeController(ILogger<HomeController> logger, ICovidCaseService covidCaseService)
        {
            _logger = logger;
            _covidCaseService= covidCaseService;
        }

        public async Task<IActionResult> Index(int? searchedDate = null, string? searchedState = null)
        {
           try
            {
                List<CovidCase> covidCases;

                if (searchedDate.HasValue)
                {
                    covidCases = await _covidCaseService.GetCasesByDate(searchedDate.Value);
                }
                else if (!string.IsNullOrWhiteSpace(searchedState))
                {
                    covidCases = await _covidCaseService.GetCasesByState(searchedState);
                }
                else
                {
                    covidCases = await _covidCaseService.GetAllCovidCases();
                }
                return View(covidCases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the " +
                    "Index method in HomeController");

                return View("Error");
            }
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
    }
}