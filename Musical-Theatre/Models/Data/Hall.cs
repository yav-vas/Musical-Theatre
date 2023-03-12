using System.ComponentModel.DataAnnotations;
using Musical_Theatre.Constants;

namespace Musical_Theatre.Models.Data
{
    public class Hall
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(DataConstants.MinHallRowsCount, DataConstants.MaxHallRowsCount)]
        public int Rows { get; set; }

        [Required]
        [Range(DataConstants.MinHallColumnsCount, DataConstants.MaxHallColumnsCount)]
        public int Columns { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}
