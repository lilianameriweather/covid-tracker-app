namespace COVIDTrackerApp.Models
{
    public class CovidCase
    {
        public int Date { get; set; }
        public string State { get; set; }
        public int? Total { get; set; }
        public int? Positive { get; set; }
        public int? Negative { get; set; }
        public int? Hospitalized { get; set; }
    }
}
