using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data.Models
{
    public class Seat
    {
        public Seat()
        {

        }

        public Seat(int id,Performance performance, int row, int seatNumber)
        {
            Id = id;
            Performance = performance;
            Row = row;
            SeatNumber = seatNumber;
        }
        public Seat(Performance performance, int row, int seatNumber)
        {
            Performance= performance;
            Row = row;
            SeatNumber= seatNumber;
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Performance))]
        public int PerformanceId { get; set; }
        public Performance Performance { get; set; }

        [Required]
        public int Row { get; set; }
        [Required]
        public int SeatNumber { get; set; }

        public Ticket? Ticket { get; set; }
    }
}
