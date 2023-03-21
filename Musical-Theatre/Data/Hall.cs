using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Constants;

namespace Musical_Theatre.Data
{
    [Index(nameof(Name), IsUnique = true)]
    public class Hall
    {
        public Hall()
        {
            this.Performances = new HashSet<Performance>();
        }
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
        public HashSet<Performance> Performances { get; set; }
    }
}
