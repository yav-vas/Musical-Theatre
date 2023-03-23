using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Musical_Theatre.Constants;
using Musical_Theatre.Data.Models.Validation;

namespace Musical_Theatre.Data.Models
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }
        public Performance()
        {
            PriceCategories = new HashSet<PriceCategory>();
        }
        [Required]
        [MaxLength(DataConstants.MaxPerformanceNameLength)]
        [UniquePerformanceName(nameof(Id))]
        public string Name { get; set; }
        public int HallId { get; set; }

        public Hall Hall { get; set; }
        public string Details { get; set; }
        public HashSet<PriceCategory> PriceCategories { get; set; }

    }
}
