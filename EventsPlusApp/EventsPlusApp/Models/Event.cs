﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusApp.Models
{
    public class Event
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Event title cannot be longer than 20 or shorter than 1 characters.")]
        [Column("Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Type cannot be longer than 20 or shorter than 1 characters.")]
        [Column("Type")]
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime DateAndTime { get; set; }
        public int LocationID { get; set; }
        public int ManagerID { get; set; }
        public Location Location { get; set; }
        public Manager Manager { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}
