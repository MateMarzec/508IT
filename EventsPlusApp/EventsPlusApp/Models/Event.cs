using System;
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
        public int EventID { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime DateAndTime { get; set; }
        public int LocationID { get; set; }
        public EventLocation EventLocation { get; set; }
        public ICollection<EventAssignment> EventAssignments { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
