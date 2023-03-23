using System.ComponentModel.DataAnnotations;
using Musical_Theatre.Constants;
using Musical_Theatre.Data.Models.Validation;

namespace Musical_Theatre.Models
{
    public class PerformanceViewModel
    {
        public int PerformanceId { get; set; }
        [Required]
        [MaxLength(DataConstants.MaxPerformanceNameLength)]
        [UniquePerformanceName(nameof(PerformanceId))]
        public string Name { get; set; }
        [Required]
        public int HallId { get; set; }
        [Required]
        public string Details { get; set; }
    }
}
