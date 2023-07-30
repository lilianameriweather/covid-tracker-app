using COVIDTrackerApp.Models;
using COVIDTrackerApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace COVIDTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidCaseApiController : ControllerBase
    {
        private readonly ILogger<CovidCaseApiController> _logger;
        private readonly ICovidCaseService _covidCaseService;
        public CovidCaseApiController(ILogger<CovidCaseApiController> logger, ICovidCaseService covidCaseService)
        {
            _logger = logger;
            _covidCaseService= covidCaseService;

        }

        [HttpGet("CovidCases")]
        public async Task<IActionResult> GetAllCovidCases()
        {
            try
            {
                List<CovidCase> covidCases = await _covidCaseService.GetAllCovidCases();

                // Order By "Positive" descending
                covidCases = covidCases.OrderByDescending(c => c.Positive).ToList();
                return Ok(covidCases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GetAllCovidCases");
                return StatusCode(500, "Error fetching data from Api");
            }
        }
    }
}
