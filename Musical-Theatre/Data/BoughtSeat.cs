using Musical_Theatre.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data
{
    public class BoughtSeat
    {
        [ForeignKey(nameof(Seat))]
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public User User { get; set; }
    }
}
