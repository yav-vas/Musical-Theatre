using System.ComponentModel.DataAnnotations;
using Musical_Theatre.Constants;

namespace Musical_Theatre.Models
{
    public class PerformanceViewModel
    {
        public int PerformanceId { get; set; }
        [Required]
        [MaxLength(DataConstants.MaxPerformanceNameLength)]
        public string Name { get; set; }
        [Required]
        public int HallId { get; set; }
        [Required]
        public string Details { get; set; }
    }
}
