using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusApp.Models
{
    public class Location
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Range(0, 1000000, ErrorMessage = "Please enter the number between 0 to 1000000.")]
        [Column("MaximumNumberofParticipants")]
        [Display(Name = "Maximum Number of Participants")]
        public int MaximumNumberofParticipants { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Location name cannot be longer than 20 or shorter than 1 characters.")]
        [Column("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
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
        [StringLength(50, MinimumLength = 1, ErrorMessage = "City cannot be longer than 50 or shorter than 1 characters.")]
        [Column("City")]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Country cannot be longer than 50 or shorter than 1 characters.")]
        [Column("Country")]
        [Display(Name = "Country")]
        public string Country { get; set; }
        
        public int OwnerID { get; set; }

        public Owner Owner { get; set; }
        public Event Event { get; set; }

    }
}
