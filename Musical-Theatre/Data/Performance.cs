using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Musical_Theatre.Constants;

namespace Musical_Theatre.Data
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }
        public Performance()
        {
            this.PriceCategories = new HashSet<PriceCategory>();
        }
        [Required]
        [MaxLength(DataConstants.MaxPerformanceNameLength)]
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(Hall))]
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public string Details { get; set; }
        public HashSet<PriceCategory> PriceCategories{ get; set; }

    }
}
