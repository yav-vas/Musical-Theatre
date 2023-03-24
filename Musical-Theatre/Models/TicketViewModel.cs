using System.ComponentModel.DataAnnotations;

namespace Musical_Theatre.Models
{
    public class TicketViewModel
    {
        public TicketViewModel()
        {
            
        }

        public TicketViewModel(string performanceName)
        {
            PerformanceName = performanceName;
        }

        [Required]
        public string PerformanceName { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int SeatNumber { get; set; } 
    }
}
