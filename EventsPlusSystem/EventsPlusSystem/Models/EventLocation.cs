using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusSystem.Models
{
    public abstract class EventLocation
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Event location name cannot be longer than 20 or shorter than 1 characters.")]
        [Column("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Range(0,1000000,ErrorMessage ="Please enter the number between 0 to 1000000.")]
        [Column("MaximumNumberofParticipants")]
        [Display(Name = "Maximum Number of Participants")]
        public int Type { get; set; }

        public LocationAssignment LocationAssignment { get; set; }
        public OwnerAssignment OwnerAssignment { get; set; }
        public LocationAddress LocationAddress { get; set; }
    }
}
