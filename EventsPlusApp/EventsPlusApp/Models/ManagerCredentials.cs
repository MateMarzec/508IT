using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusApp.Models
{
    public class ManagerCredentials : EventManager
    {
        [ForeignKey("ManagerID")]
        public int ManagerID { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Username cannot be longer than 15 or shorter than 7 characters.")]
        [Column("Username")]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Password cannot be longer than 15 or shorter than 7 characters.")]
        [Column("Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter valid email.")]
        [Column("Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public EventManager EventManager { get; set; }
    }
}
