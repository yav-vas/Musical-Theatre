using System.ComponentModel.DataAnnotations;
using Musical_Theatre.Constants;

namespace Musical_Theatre.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Range(DataConstants.MinHallRowsCount, DataConstants.MaxHallRowsCount)]
        public int Rows { get; set; }
        [Range(DataConstants.MinHallColumnsCount, DataConstants.MaxHallColumnsCount)]
        public int Columns { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}
