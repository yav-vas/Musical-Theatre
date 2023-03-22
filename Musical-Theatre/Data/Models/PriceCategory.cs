using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Musical_Theatre.Constants;

namespace Musical_Theatre.Data.Models
{
    public class PriceCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(DataConstants.MinPriceValue, double.MaxValue)]
        public double Price { get; set; }
        [ForeignKey(nameof(Performance))]
        public int PerformanceId { get; set; }
        public Performance Performance { get; set; }
    }
}
