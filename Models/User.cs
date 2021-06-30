using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set;}

        [Required(ErrorMessage = "is required")]
        [MinLength(2)]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "is required")]
        [MinLength(2)]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "is required")]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "is required")]
        [Compare("Password",ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        [Display(Name="Confrim Password")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<WatchedMovie> WatchedMovies { get; set; }
        public List<UnwatchedMovie> UnwatchedMovies { get; set; }
    }
}