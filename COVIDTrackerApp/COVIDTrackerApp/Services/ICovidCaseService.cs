using COVIDTrackerApp.Models;

namespace COVIDTrackerApp.Services
{
    public interface ICovidCaseService
    {
        public Task<List<CovidCase>> GetAllCovidCases();
        public Task<List<CovidCase>> GetCasesByDate(int searchDate);
        public Task<List<CovidCase>> GetCasesByState(string searchState);

    }
}
