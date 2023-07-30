using COVIDTrackerApp.Models;
using Newtonsoft.Json;

namespace COVIDTrackerApp.Services
{
    public class CovidCaseService : ICovidCaseService
    {

        private readonly ILogger<CovidCaseService> _logger;
        private readonly HttpClient client;

        public CovidCaseService(ILogger<CovidCaseService> logger, HttpClient httpClient)
        {
            _logger = logger;
            client = httpClient;
        }

        public async Task<List<CovidCase>> GetAllCovidCases()
        {
            List<CovidCase> covidCases = new List<CovidCase>();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.covidtracking.com/v1/states/daily.json");
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                
                covidCases = JsonConvert.DeserializeObject<List<CovidCase>>(content);
                covidCases = covidCases.OrderBy(c => c.State).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GetAllCovidCases");
            }

            return covidCases;
        }

        public async Task<List<CovidCase>> GetCasesByDate(int filterDate)
        {
            List<CovidCase> covidCases = new List<CovidCase>();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.covidtracking.com/v1/states/daily.json");
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                covidCases = JsonConvert.DeserializeObject<List<CovidCase>>(content);

                // Filter list by provided date
                covidCases = covidCases.Where(cc => cc.Date == filterDate).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GetCasesByDate");
            }

            return covidCases;
        }

        public async Task<List<CovidCase>> GetCasesByState(string filterState)
        {
            List<CovidCase> covidCases = new List<CovidCase>();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.covidtracking.com/v1/states/daily.json");
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                covidCases = JsonConvert.DeserializeObject<List<CovidCase>>(content);

                // Filter list by provided State
                covidCases = covidCases.Where(cc => cc.State.Equals(filterState)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GetCasesByState");
            }

            return covidCases;
        }


    }
}
