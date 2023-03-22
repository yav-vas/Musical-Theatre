using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string? Checker_Id { get; set; }
        public User User { get; set; }
        [Required]
        [ForeignKey(nameof(Seat))]
        public int? Seat_Id { get; set; }
        public Seat Seat { get; set; }
    }
}
