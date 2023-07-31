using System.ComponentModel.DataAnnotations;

namespace COVIDTrackerApp.Models
{
    public class CovidCase
    {
        [Required]
        public int Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string State { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Total { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Positive { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Negative { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Hospitalized { get; set; }
    }
}
