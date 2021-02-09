using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusSystem.Models
{
    public abstract class EventParticipant
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "First name cannot be longer than 50 or shorter than 2 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Last name cannot be longer than 50 or shorter than 2 characters.")]
        [Column("LastName")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            { return FirstName + " " + LastName; }
        }
        [Required]
        [StringLength(14, MinimumLength = 10, ErrorMessage = "Phone number cannot be longer than 14 or shorter than 10 characters.")]
        [Column("PhoneNumber")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public EventParticipantCredentials EventParticipantCredentials { get; set; }
    }
}
