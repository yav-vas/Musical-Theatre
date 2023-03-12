using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Performance))]
        public int PerformanceId { get; set; }
        public Performance Performance { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int SeatNumber { get; set; }
        [ForeignKey(nameof(PriceCategory))]
        public int PriceCategoryId { get; }
        public PriceCategory PriceCategory { get; set; }
    }
}
