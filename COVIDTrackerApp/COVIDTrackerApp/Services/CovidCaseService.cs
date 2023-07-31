using System.Net.Http;
using COVIDTrackerApp.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace COVIDTrackerApp.Services
{
    public class CovidCaseService : ICovidCaseService
    {
        private readonly ILogger<CovidCaseService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly string _apiUrl = "https://api.covidtracking.com/v1/states/daily.json";


        public CovidCaseService(
            ILogger<CovidCaseService> logger,
            HttpClient httpClient, IMemoryCache memoryCache)
        {
            _logger = logger;
            _httpClient = httpClient;
            _memoryCache = memoryCache;

        }

        public async Task<List<CovidCase>> GetAllCovidCasesAsync()
        {
            List<CovidCase> covidCases = new List<CovidCase>();

            try
            {
                // only ony httpclient
                HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                covidCases = JsonConvert.DeserializeObject<List<CovidCase>>(content);
                covidCases = covidCases.OrderBy(c => c.State).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Covid cases from the API.");
                throw; // Rethrow the exception to handle it at the caller level if needed.
            }

            return covidCases;
        }

        public async Task<List<CovidCase>> GetAllCovidCases()
        {
            // Check if the data is already cached before making a new API request.
            if (!_memoryCache.TryGetValue("AllCovidCases", out List<CovidCase> covidCases))
            {
                // If not cached, retrieve data from the API and cache it for future requests.
                covidCases = await GetAllCovidCasesAsync();
                _memoryCache.Set("AllCovidCases", covidCases, TimeSpan.FromHours(1)); // Cache for 1 hour.
            }

            return covidCases;
        }

        public async Task<List<CovidCase>> GetCasesByDate(int filterDate)
        {
            var allCovidCases = await GetAllCovidCases();
            return allCovidCases.Where(cc => cc.Date == filterDate).ToList();
        }

        public async Task<List<CovidCase>> GetCasesByState(string filterState)
        {
            var allCovidCases = await GetAllCovidCases();
            return allCovidCases.Where(cc => cc.State.Equals(filterState, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        //public async Task<List<CovidCase>> GetCasesByDate(int filterDate)
        //{
        //    List<CovidCase> covidCases = new List<CovidCase>();

        //    try
        //    {
        //        HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl);
        //        response.EnsureSuccessStatusCode();

        //        string content = await response.Content.ReadAsStringAsync();

        //        covidCases = JsonConvert.DeserializeObject<List<CovidCase>>(content);

        //        // Filter list by provided date
        //        covidCases = covidCases.Where(cc => cc.Date == filterDate).ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing GetCasesByDate");
        //    }

        //    return covidCases;
        //}

        //public async Task<List<CovidCase>> GetCasesByState(string filterState)
        //{
        //    List<CovidCase> covidCases = new List<CovidCase>();

        //    try
        //    {
        //        HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl);
        //        response.EnsureSuccessStatusCode();

        //        string content = await response.Content.ReadAsStringAsync();

        //        covidCases = JsonConvert.DeserializeObject<List<CovidCase>>(content);

        //        // Filter list by provided State
        //        covidCases = covidCases.Where(cc => cc.State.Equals(filterState)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing GetCasesByState");
        //    }

        //    return covidCases;
        //}


    }
}
