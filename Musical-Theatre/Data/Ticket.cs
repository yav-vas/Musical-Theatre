using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int? Checker_Id { get; set; }
        [Required]
        [ForeignKey(nameof(Seat))]
        public int? Seat_Id { get; set; }

    }
}
