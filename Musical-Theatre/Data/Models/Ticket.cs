using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int SeatId { get; set; }
        public Seat Seat { get; set; }
    }
}
