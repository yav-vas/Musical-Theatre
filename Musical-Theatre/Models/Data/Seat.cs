using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Models.Data
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Performance))]
        public int Performance_Id { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int Seat_Number { get; set; }
        [Required]
        [ForeignKey(nameof(PriceCategory))]
        public int Price_Category_Id { get;}
    }
}
