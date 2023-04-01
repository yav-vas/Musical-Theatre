using Musical_Theatre.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Musical_Theatre.Models
{
    public class TicketViewModel
    {
        public TicketViewModel()
        {
            Seats = new List<List<Seat>>();
        }

        public TicketViewModel(string performanceName)
        {
            PerformanceName = performanceName;
            Seats = new List<List<Seat>>();
        }

        public TicketViewModel(string performanceName, List<List<Seat>> seats, Hall hall)
        {
            Seats = seats;
            PerformanceName = performanceName;
            Hall = hall;
        }

        public TicketViewModel(string performanceName, int row, int seatNumber)
        {
            PerformanceName = performanceName;
            Row = row;
            SeatNumber = seatNumber;
        }

        public int TicketId { get; set; }

        public List<List<Seat>> Seats { get; set; }

        public Hall Hall { get; set; }

        [Required]
        public string PerformanceName { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        [Display(Name = "Seat number")]
        public int SeatNumber { get; set; } 
    }
}
