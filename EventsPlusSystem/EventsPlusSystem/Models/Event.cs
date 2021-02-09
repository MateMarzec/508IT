using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusSystem.Models
{
    public class Event
    {
        public int ID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Event name cannot be longer than 20 or shorter than 1 characters.")]
        [Column("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Type cannot be longer than 20 or shorter than 1 characters.")]
        [Column("Type")]
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime DateAndTime { get; set; }

        public LocationAssignment LocationAssigment { get; set; }
        public EventAssignment EventAssignment { get; set; }
        public Booking Booking { get; set; }
    }
}
