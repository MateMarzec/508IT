using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusSystem.Models
{
    public class LocationAddress : EventLocation
    {
        public int UserID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Event location name cannot be longer than 20 or shorter than 1 characters.")]
        [Column("EventLocationName")]
        [Display(Name = "Event Location Name")]
        public string EventLocationName { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Post code cannot be longer than 10 or shorter than 3 characters.")]
        [Column("PostCode")]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Address cannot be longer than 50 or shorter than 2 characters.")]
        [Column("Address")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Country cannot be longer than 50 or shorter than 1 characters.")]
        [Column("Country")]
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "City cannot be longer than 50 or shorter than 1 characters.")]
        [Column("City")]
        [Display(Name = "City")]
        public string City { get; set; }

        public EventLocation EventLocation { get; set; }
    }
}
