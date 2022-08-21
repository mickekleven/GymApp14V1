﻿using System.ComponentModel.DataAnnotations;

namespace GymApp14V1.Models.ViewModels
{
#nullable disable
    public class GymClassViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Gym class name")]
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime => StartTime + Duration;
        public string Description { get; set; }
    }
}
