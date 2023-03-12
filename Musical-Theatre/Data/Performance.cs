using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musical_Theatre.Data
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Hall))]
        public int Hall_Id { get; set; }

        public string Details { get; set; }

    }
}
