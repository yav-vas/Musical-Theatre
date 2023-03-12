using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data
{
    public class BoughtSeats
    {
        [ForeignKey(nameof(Seat))]
        public int SeatId { get; set; }
        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }
        [ForeignKey(nameof(User))]
        public int OwnerId { get; set; }
    }
}
