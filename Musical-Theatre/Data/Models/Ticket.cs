using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Checker))]
        public string? CheckerId { get; set; }
        public User? Checker { get; set; }

        [ForeignKey(nameof(Owner))]
        public string? OwnerId { get; set; }
        public User? Owner { get; set; }

        [Required]
        public int? SeatId { get; set; }
        public Seat Seat { get; set; }
    }
}
