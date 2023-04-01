using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Musical_Theatre.Constants;
using Musical_Theatre.Data.Models.Validation;

namespace Musical_Theatre.Data.Models
{
    public class Hall
    {
        public Hall()
        {
            Performances = new HashSet<Performance>();
        }
        public Hall(int id, string name, int rows, int columns, DateTime dateCreated)
        {
            Id = id;
            Name = name;
            Rows = rows;
            Columns = columns;
            DateCreated = dateCreated;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [UniqueHallNameAttribute(nameof(Id))]
        public string Name { get; set; }

        [Required]
        [Range(DataConstants.MinHallRowsCount, DataConstants.MaxHallRowsCount)]
        public int Rows { get; set; }

        [Required]
        [Range(DataConstants.MinHallColumnsCount, DataConstants.MaxHallColumnsCount)]
        public int Columns { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; }

        public HashSet<Performance> Performances { get; set; }
    }
}
