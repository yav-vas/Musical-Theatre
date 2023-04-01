using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Musical_Theatre.Constants;
using Musical_Theatre.Data.Models.Validation;

namespace Musical_Theatre.Data.Models
{
    public class Performance
    {
        public Performance()
        {
        }

        public Performance(int id, string name, int hallId, string details)
        {
            Id = id;
            Name = name;
            HallId = hallId;
            Details = details;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(DataConstants.MaxPerformanceNameLength)]
        [UniquePerformanceName(nameof(Id))]
        public string Name { get; set; }
        public int HallId { get; set; }

        public Hall Hall { get; set; }
        public string Details { get; set; }

    }
}
