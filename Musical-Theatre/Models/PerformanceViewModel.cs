﻿using System.ComponentModel.DataAnnotations;
using Musical_Theatre.Constants;
using Musical_Theatre.Data.Models.Validation;

namespace Musical_Theatre.Models
{
    public class PerformanceViewModel
    {
        public PerformanceViewModel()
        {

        }

        public PerformanceViewModel(int performanceId, string name, int hallId, string details)
        {
            PerformanceId = performanceId;
            Name = name;
            HallId = hallId;
            Details = details;
        }

        public int PerformanceId { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxPerformanceNameLength)]
        [UniquePerformanceName(nameof(PerformanceId))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Hall name")]
        public int HallId { get; set; }

        [Required]
        public string Details { get; set; }
    }
}
